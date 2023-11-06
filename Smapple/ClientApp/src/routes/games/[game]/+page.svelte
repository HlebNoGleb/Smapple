<script lang="ts">
// @ts-nocheck
    import Swal from 'sweetalert2'
    import { page } from "$app/stores";
    import { user } from "$lib/user"
    import { gameStatus, gameUserStatus, gameTypes } from '$lib/enums.js';

    export let data;

    console.log(data);

    const currentStatus = Object.values(gameStatus).find(x => x.id == data.game.status);
    const gameType = Object.values(gameTypes).find(x => x.id == data.game.type);
    const isOpenGame = gameType.id == gameTypes.Public.id;
    const isUserInGame = data.game.gameUsers.find(x => x.userId == user?.data.id) != undefined;
    const isUserHost = data.game.hostId == user?.data.id;
    let userScore = data?.game?.gameUsers.find(x=>x.userId == user?.data.id)?.userScore || 0;


    let editGameForm = {
      title: data.game.name || "",
      datetime: data.game.gameDateTime || null,
      slots: data.game.slotsCount || 0,
      address: data.game.address || "",
      type: `${data.game.type}` || "0",
      imageUrl: data.game.image || "",
      hostId: data.game.hostId
    }

    function update() {
      if (typeof window == "undefined") {
        return;
      }

      location.reload();
    }

    async function submit() {
      const response = await fetch(`/api/game/${$page.params.game}`, {
        method: 'PUT',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${user.token}`
        },
        body: JSON.stringify(editGameForm)
      });

      if (response.ok) {
        Swal.fire({
          title: 'Отлично',
          text: 'Данные игры обновлены',
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

    function setYandexMaps(node) {
      try {
        console.log(node)
        ymaps.ready(init);
        const lat = node.getAttribute("data-lat")
        const long = node.getAttribute("data-long")
        const name = node.getAttribute("data-name")


        function init() {
          var myMap = new ymaps.Map('map', {
            center: [lat, long],
            zoom: 15,
            controls: ["zoomControl", "fullscreenControl"]
          });

          var pin = new ymaps.Placemark(
            [parseFloat(lat), parseFloat(long)], {
              iconCaption: name
            }, {
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

    async function removeUserFromGame(gameId, userId) {

      const response = await fetch(`/api/game/${gameId}/removeUser/${userId}`, {
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
          icon: 'error',
          confirmButtonText: 'Ок'
        })

        throw new Error(`responce error.status: ${response.status}. ${response.statusText}`);
      }
    }

    async function declineUserToGame(gameId, userId) {
      const response = await fetch(`/api/game/${gameId}/declineuser/${userId}`, {
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
          icon: 'error',
          confirmButtonText: 'Ок'
        })

        throw new Error(`responce error.status: ${response.status}. ${response.statusText}`);
      }
    }

    async function savePoints() {
      const response = await fetch(`/api/game/${$page.params.game}/addPoints`, {
        method: 'POST',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${user.token}`
        },
        body: userScore
      });

      if (response.ok) {
        Swal.fire({
          title: 'Отлично',
          text: 'Ваши очки сохранены',
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
</script>

{#if data}
<main>
  <div class="game" style="background:linear-gradient(0deg, rgba(0,0,0,.7), rgba(0,0,0,.7)), url({data.game.image});">
<div class="container">
      <h1 class="text-body-emphasis">{data.game.name}  {#if data.game.type == gameTypes.Private.id}&#128274;{/if}</h1>
      <p class="fs-5 col-md-8">{new Date(data.game.gameDateTime).toLocaleDateString()} - {new Date(data.game.gameDateTime).toLocaleTimeString()}</p>
      <p class="fs-5 badge rounded-pill {currentStatus?.cssClass}">Статус: {currentStatus.text}</p>
      <p class="fs-5 col-md-8">Адрес: {data.game.address}</p>
      {#if user}
        {#if !isUserInGame && !isUserHost}
          <div class="mb-5">
              <button class="btn btn-primary btn-lg px-4" on:click={join}>Присоединиться</button>
          </div>
        {/if}
        {#if isUserInGame && isOpenGame && currentStatus.id == gameStatus.Opened.id}
          <div class="mb-5">
              <button class="btn btn-danger btn-lg px-4" on:click={() => {declineUserToGame(data.game.id, user?.data.id)}}>Выйти из игры</button>
          </div>
        {/if}
        {#if isUserInGame && !isOpenGame && currentStatus.id == gameStatus.Opened.id}
          <div class="mb-5">
              <button class="btn btn-danger btn-lg px-4" on:click={() => {removeUserFromGame(data.game.id, user?.data.id)}}>Отменить заявку</button>
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
            {:else}
            <li>
              <a class="mb-1 text-white-50" href="/users/{item.userId}">
                {item.user.nickName}
              </a>
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
    {#if currentStatus.id == gameStatus.CountingResults.id && data?.game?.gameUsers.find(x=>x.userId == user?.data.id)}
      <div class="col-md-6">
        <div class="col-sm-6">
          <label for="points" class="form-label">Внесите ваши очки</label>
          <input bind:value={userScore} type="number" class="form-control" id="points" placeholder="" >
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
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Закрыть</button>
                    <button type="submit" class="btn btn-primary">Сохранить</button>
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