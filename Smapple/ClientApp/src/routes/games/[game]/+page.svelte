<svelte:head>
    <!-- <script src="https://api-maps.yandex.ru/2.1/?lang=ru_RU" type="text/javascript"></script> -->
</svelte:head>

<script lang="ts">
// @ts-nocheck
    import Swal from 'sweetalert2'
    import { page } from "$app/stores";
    import { user } from "$lib/user"

    export let data;


    console.log(data)

    // console.log(user?.data)
    // console.log(data.game)

    let gameStatus = {
      Opened: {
        id: 0,
        text: "Открытая"
      },
      InProgress: {
        id: 1,
        text: "В процессе"
      },
      CountingResults: {
        id: 2,
        text: "Подсчет очков"
      },
      Closed: {
        id: 3,
        text: "Закрытая"
      },
    }

    let gameUserStatus = {
      Pending: {
        id: 0,
        text: "Запрос"
      },
      Approved: {
        id: 1,
        text: "Подтвержден"
      },
      Declined: {
        id: 2,
        text: "Запрещен"
      },
    }

    let currentStatus = Object.values(gameStatus).find(x=>x.id == data.game.status);

    console.log(currentStatus)

    let editGameForm = {
      title: "",
      datetime: null,
      slots: 0,
      address: "",
      type: "0",
      imageUrl: ""
    }

  function update() {
    if (typeof window == "undefined") {
      return;
    }

    location.reload();
  }

  function submit() {
    alert(JSON.stringify(editGameForm));
    // send to server
    // if 200ok
    update()
  }

    function setYandexMaps(node) {
        try {
            console.log(node)
            ymaps.ready(init);
            const lat = node.getAttribute("data-lat")
            const long = node.getAttribute("data-long")
            const name = node.getAttribute("data-name")


                function init () {
                    var myMap = new ymaps.Map('map',
                    {
                        center: [lat, long],
                        zoom: 15,
                        controls: ["zoomControl", "fullscreenControl"]
                    });

                    var pin = new ymaps.Placemark(
                    [parseFloat(lat), parseFloat(long)],
                    {
                        iconCaption: name
                    },
                    {
                        openBalloonOnClick: false,
                        preset: 'islands#blueCircleDotIcon'
                    });

                    myMap.geoObjects.add(pin);
                }
        } catch (error) {
            console.log(error)
        }

    }

    async function startGame() {
      const response = await fetch(`/api/game/${$page.params.game}/start`, {
        method: 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${user.token}`
        },
      });

      if (response.ok) {
        Swal.fire({
            title: 'Отлично',
            text: 'Игра началась',
            icon: 'success',
            confirmButtonText: 'Ок'
          }).then((result) => {
              if (result.isConfirmed) {
                update()
              }
            })
      }

      if (!response.ok) {
        throw new Error(`responce error.status: ${response.status}. ${response.statusText}`);
      }
    }



    async function endGame() {
      const response = await fetch(`/api/game/${$page.params.game}/counting`, {
        method: 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${user.token}`
        },
      });

      if (response.ok) {
        Swal.fire({
            title: 'Отлично',
            text: 'Игра завершилась. Начинайте подсчет очков',
            icon: 'success',
            confirmButtonText: 'Ок'
          }).then((result) => {
              if (result.isConfirmed) {
                update()
              }
            })
      }

      if (!response.ok) {
        Swal.fire({
            title: 'Ошибка',
            text: 'Произошла ошибка',
            icon: 'error',
            confirmButtonText: 'Ок'
          })
        throw new Error(`responce error.status: ${response.status}. ${response.statusText}`);
      }
    }

    async function saveGame() {
      const response = await fetch(`/api/game/${$page.params.game}/close`, {
        method: 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${user.token}`
        },
      });

      if (response.ok) {
        Swal.fire({
            title: 'Отлично',
            text: 'Игра завершена. Спасибо за игру',
            icon: 'success',
            confirmButtonText: 'Ок'
          }).then((result) => {
              if (result.isConfirmed) {
                update()
              }
            })
      }

      if (!response.ok) {
        throw new Error(`responce error.status: ${response.status}. ${response.statusText}`);
      }


    }

    let points = 0;

    async function join() {
      const response = await fetch(`/api/game/${$page.params.game}/createApplication`, {
        method: 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${user.token}`
        },
      });

      if (response.ok) {
        Swal.fire({
            title: 'Отлично',
            text: 'Вы присоеденились к игре',
            icon: 'success',
            confirmButtonText: 'Ок'
          })
      }

      if (!response.ok) {
        throw new Error(`responce error.status: ${response.status}. ${response.statusText}`);
      }

      update()
  }

    async function savePoints() {
      const response = await fetch(`/api/game/${$page.params.game}/addPoints`, {
        method: 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${user.token}`
        },
        body:points
      });

      if (response.ok) {
        Swal.fire({
            title: 'Отлично',
            text: 'Ваши очки сохранены',
            icon: 'success',
            confirmButtonText: 'Ок'
          })
      }

      if (!response.ok) {
        throw new Error(`responce error.status: ${response.status}. ${response.statusText}`);
      }

      update()
    }
</script>

{#if data}
<main>
  <div class="game" style="background:linear-gradient(0deg, rgba(0,0,0,.7), rgba(0,0,0,.7)), url({data.game.image});">
<div class="container">
      <h1 class="text-body-emphasis">{data.game.name}</h1>
      <p class="fs-5 col-md-8">{new Date(data.game.gameDateTime).toLocaleDateString()} - {new Date(data.game.gameDateTime).toLocaleTimeString()}</p>
      <p class="fs-5 col-md-8">Статус: {currentStatus.text}</p>
      <p class="fs-5 col-md-8">Адрес: {data.game.address}</p>
      <p class="fs-5 col-md-8">тип: {data.game.type}</p>


      {#if user}
        {#if data.game.gameUsers && !data?.game?.gameUsers.find(x=>x.userId == user.data.id) && data.game.hostId != user.data.id}
          <div class="mb-5">
              <button class="btn btn-primary btn-lg px-4" on:click={join}>Присоединиться</button>
          </div>
        {/if}
      {/if}
      {#if data.game.hostId == user?.data.id || user?.data.nickName == "admin"}
        <div class="my-3">
          {#if currentStatus.id == gameStatus.Opened.id}
            <button type="button" class="btn btn-primary" on:click={startGame}>
                Начать игру
            </button>
            <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#exampleModal">
              Изменить игру
            </button>
          {/if}
          {#if currentStatus.id == gameStatus.InProgress.id}
            <button type="button" class="btn btn-primary" on:click={endGame}>
                Завершить игру
            </button>
          {/if}
          {#if currentStatus.id == gameStatus.CountingResults.id}
            <button type="button" class="btn btn-primary" on:click={saveGame}>
                Сохранить данные игроков и закрыть игру
            </button>
          {/if}
          </div>
        {/if}
      <hr class="col-3 col-md-2 mb-5">
      </div>
  </div>
  <div class="container">

  <div class="row g-5 my-3">
    {#if data.game.gameUsers && data.game.gameUsers.length > 0}
      <div class="col-md-6">
        <h2 class="text-body-emphasis">Список игроков</h2>
        <ul class="list-unstyled ps-0">
          {#each data.game.gameUsers as item}
            {#if item.status == gameUserStatus.Approved.id}
            <li>
              <a class="mb-1 text-white" href="/users/{item.userId}">
                {item.user.nickName}
              </a>
              {#if currentStatus.id == gameStatus.CountingResults.id || currentStatus.id == gameStatus.Closed.id}
                <span>Очки за игру: {item.userScore}</span>
              {/if}
            </li>
            {/if}
          {/each}
        </ul>
      </div>
    {/if}
    <div class="col-md-6">
      <!-- <div id="map" style="width: 100%; height: 400px" data-lat="57" data-long="22" data-name="название игры" use:setYandexMaps></div> -->
    </div>
  </div>
  <div class="row g-5 my-3">
    {#if currentStatus.id == gameStatus.CountingResults.id && data?.game?.gameUsers.find(x=>x.userId == user.data.id)}
      <div class="col-md-6">
        <div class="col-sm-6">
          <label for="points" class="form-label">Внесите ваши очки</label>
          <input bind:value={points} type="number" class="form-control" id="points" placeholder="" >
        </div>
        <button type="button" class="btn btn-light my-3" on:click={savePoints}>
          Сохранить
        </button>
      </div>
      {/if}
    </div>
  </div>
</main>

<!-- Modal -->
<div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content substrate">
        <div class="modal-header">
          <h1 class="modal-title fs-5" id="exampleModalLabel">Modal title</h1>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>
        <div class="modal-body">
            <form id="addform" on:submit|preventDefault={submit}>
                <div class="row g-3">
                  <div class="col-sm-6">
                    <label for="title" class="form-label">Название игры</label>
                    <input bind:value={editGameForm.title} type="text" class="form-control" id="title" placeholder="" >
                  </div>

                  <div class="col-sm-6">
                    <label for="datetime" class="form-label">Дата и время</label>
                    <input bind:value={editGameForm.datetime} type="datetime-local" class="form-control" id="datetime" placeholder="" >
                  </div>

                  <div class="col-12">
                    <label for="slots" class="form-label">Кол-во мест</label>
                    <input bind:value={editGameForm.slots} type="number" class="form-control" id="slots" placeholder="" >
                  </div>

                  <div class="col-12">
                    <label for="address" class="form-label">Адрес</label>
                    <input bind:value={editGameForm.address} type="text" class="form-control" id="address" placeholder="" >
                  </div>

                  <div class="col-12">
                    <label for="image" class="form-label">Фон</label>
                    <input bind:value={editGameForm.imageUrl} type="text" class="form-control" id="image" placeholder="" >
                  </div>

                  {#if editGameForm.imageUrl}
                    <img src="{editGameForm.imageUrl}" style="max-width: 100%;" alt=""/>
                  {/if}

                  <div class="col-12">
                    <label for="country" class="form-label">Тип игры</label>
                    <select bind:value={editGameForm.type}>
                      <option value="0">Открытая</option>
                      <option value="1">Закрытая</option>
                    </select>
                  </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                    <button type="submit" class="btn btn-primary">Save changes</button>
                  </div>
              </form>
        </div>

      </div>
    </div>
</div>
{/if}

<style>
    .game{
        position: relative;
        background-size: cover!important;
        padding: 20px;
        color: #fff;
        background-position: center!important;
    }
</style>