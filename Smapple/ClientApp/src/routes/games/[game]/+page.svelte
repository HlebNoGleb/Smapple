<svelte:head>
    <!-- <script src="https://api-maps.yandex.ru/2.1/?lang=ru_RU" type="text/javascript"></script> -->
</svelte:head>

<script lang="ts">
// @ts-nocheck
    import { page } from "$app/stores";
    import { user } from "$lib/user"

    export let data;

    console.log(user)
    console.log(data)

    let gameStatus = [
      {id: 0, text: "Открытая"},
      {id: 1, text: "В процессе"},
      {id: 2, text: "Подсчет очков"},
      {id: 3, text: "Завершена"}
    ];

    let currentStatus = gameStatus.find(x=>x.id == data.game.status);

    let editGameForm = {
      title: "",
      datetime: null,
      slots: 0,
      address: "",
      type: "0",
      imageUrl: ""
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

    function startGame() {
        alert(`игра ${$page.params.game} началась. статус inprogress`)
    }

    function endGame() {
        alert(`игра ${$page.params.game} завешилась. статус countResult`)
    }

    function saveGame() {
        alert(`игра ${$page.params.game} завешилась и результаты пользователей сохранены. статус closed`)
    }

    let points = 0;

    async function join() {
      const response = await fetch(`/api/game/${$page.params.game}/createApplication`, {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        'Authorization': `Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIyIiwicm9sZSI6Ikhvc3QiLCJuYmYiOjE2OTg1Mzk3MjgsImV4cCI6MTY5OTE0NDUyOCwiaWF0IjoxNjk4NTM5NzI4fQ.TwI1EvEuPrOJNfqmrZfDdGywoePnLWIpizUNupxdVlod76yIlsw2ud8qE78M8Mf6F-KGhqj2n5ViEFY2QRqHzQ`
      },
    });

    if (!response.ok) {
      if (respomse.code == 401) {
        Swal.fire({
          title: 'Error!',
          text: 'Do you want to continue',
          icon: 'error',
          confirmButtonText: 'Cool'
        })
      }
        throw new Error(`HTTP error! status: ${response.status}.`);
      }
    }

    function savePoints() {
      alert(`текущий пользователь из сессии сохранил ${points} очков`)
    }
</script>

<main>
  <div class="game" style="background:linear-gradient(0deg, rgba(0,0,0,.7), rgba(0,0,0,.7)), url({data.game.image});">
<div class="container">
      <h1 class="text-body-emphasis">{data.game.name}</h1>
      <p class="fs-5 col-md-8">{new Date(data.game.gameDateTime).toLocaleDateString()} - {new Date(data.game.gameDateTime).toLocaleTimeString()}</p>
      <p class="fs-5 col-md-8">Статус: {currentStatus.text}</p>
      {#if data.game.gameUsers && data?.game?.gameUsers.find(x=>x.id == user.data.id)}
        <div class="mb-5">
            <button class="btn btn-primary btn-lg px-4" on:click={join}>Присоединиться</button>
        </div>
      {/if}
      <!-- {#if data.game.hostId == user?.data.id || user?.data.nickName == "admin"}
        <div class="my-3">
            <button type="button" class="btn btn-primary" on:click={startGame}>
                Начать игру
            </button>
            <button type="button" class="btn btn-primary" on:click={endGame}>
                Завершить игру
            </button>
            <button type="button" class="btn btn-primary" on:click={saveGame}>
                Сохранить данные игроков и закрыть игру
            </button>
            <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#exampleModal">
                Изменить игру
            </button>
          </div>
        {/if} -->
      <hr class="col-3 col-md-2 mb-5">
      </div>
  </div>
  <div class="container">

  <div class="row g-5 my-3">
    <div class="col-md-6">
      <h2 class="text-body-emphasis">Список игроков</h2>
      <ul class="list-unstyled ps-0">
        {#each [1,2,3,4] as item}
        <li>
          <a class="mb-1" href="/users/{item}">
            Nickname {item}
          </a>
        </li>
        {/each}
      </ul>
    </div>
    <div class="col-md-6">
      <!-- <div id="map" style="width: 100%; height: 400px" data-lat="57" data-long="22" data-name="название игры" use:setYandexMaps></div> -->
    </div>
  </div>
  <div class="row g-5 my-3">
      <div class="col-md-6">
        <h2 class="text-body-emphasis">Если статус countResult</h2>
        <div class="col-sm-6">
          <label for="points" class="form-label">Внесите ваши очки</label>
          <input bind:value={points} type="text" class="form-control" id="points" placeholder="" >
        </div>
        <button type="button" class="btn btn-primary" on:click={savePoints}>
          Сохранить
        </button>
      </div>
    </div>
  </div>
</main>

<!-- Modal -->
<div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content">
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

<style>
    .game{
        position: relative;
        background-size: cover!important;
        padding: 20px;
        color: #fff;
        background-position: center!important;
    }
</style>