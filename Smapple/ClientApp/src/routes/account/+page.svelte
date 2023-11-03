<script>
// @ts-ignore
    import Swal from 'sweetalert2'
    import { onMount } from 'svelte';
    import { user } from "$lib/user"
    export let data;

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

    let userModel = {
      myHistory: [],
      myHosted: [],
      myApplications: [],
    }

    function init() {
      userModel.myHistory = data.me?.gameUsers?.filter(x=>x.status == gameUserStatus.Approved.id) ?? [];
      userModel.myHosted = data.me.hostedGames ?? [];
      userModel.myApplications = data.me?.gameUsers?.filter(x=>x.status == gameUserStatus.Pending.id) ?? [];
      console.log(data.me, userModel)
    }

    function update() {
      if (typeof window == "undefined") {
        return;
      }

      location.reload();
    }

    let userData = {
        name: "",
        description:"",
    }

    onMount(() => {
        init();
        if (user) {
          userData.name = user.data.nickName
          userData.description = user.data.description
        }
    })



    function submit() {
        alert(JSON.stringify(userData));
    }

    async function acceptUserToGame(gameId, userId) {
      const response = await fetch(`/api/game/${gameId}/submitUser/${userId}`, {
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
        })
        update()
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
          })
        update()

      }

      if (!response.ok) {
        Swal.fire({
            title: 'Ошибка',
            icon: 'error',
            confirmButtonText: 'Ок'
          }).then({

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
          })
        update()

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

</script>
{#if data.me}
<main>
    <div class="user">
      <div class="container">
        <h1 class="text-body-emphasis">{data.me.nickName}</h1>
        <h1 class="text-body-emphasis">Очки: {data.me.points}</h1>
        <p class="fs-5 col-md-8">{data.me.email}</p>
        <hr class="col-3 col-md-2 mb-5">
          <div class="my-3">
            <button type="button" class="btn btn-light" data-bs-toggle="modal" data-bs-target="#exampleModal">
              Изменить данные
            </button>
          </div>
      </div>
    </div>
    <div class="container">

      <div class="row g-5 my-3">
        {#if userModel.myHistory.length > 0}
        <div class="col-md-6">
          <h2 class="text-body-emphasis">История игр</h2>
          <ul class="list-unstyled ps-0">
            {#each userModel.myHistory as history}
              <li>
                <a class="mb-1" href="/games/{history.game.id}">
                  {history.game.name}
                </a>
                <span>Очки: {history.userScore}</span>
                <button type="button" class="btn btn-danger" on:click={() => {removeUserFromGame(history.game.id, user?.data.id)}}>
                    Отменить участие в игре
                </button>
              </li>
            {/each}
          </ul>
        </div>
        {/if}
        {#if userModel.myApplications.length > 0}
        <div class="col-md-6">
          <h2 class="text-body-emphasis">Заявки</h2>
          <ul class="list-unstyled ps-0">
            {#each userModel.myApplications as application}
              <li>
                <a class="mb-1" href="/games/{application.game.id}">
                  {application.game.name}
                </a>
                <span>Очки: {application.userScore}</span>
                <button type="button" class="btn btn-danger" on:click={() => {removeUserFromGame(application.gameId, application.userId)}}>
                    Отменить заявку {application.userId}. Игра {application.gameId}
                </button>
              </li>
            {/each}
          </ul>
        </div>
        {/if}
      <div class="row g-5 my-3">
        <div class="col-md-12">
          {#if userModel.myHosted.length > 0}
          <h2 class="text-body-emphasis">История ведения игр</h2>
          <ul class="list-unstyled ps-0">
            {#each userModel.myHosted as hosted}
            <li>
              <a class="mb-1" href="/games/{hosted.id}">
                {hosted.name}
              </a>
              <ul class="list-unstyled ps-0">
                {#each hosted.gameUsers as gameUser}
                <li>
                  <a class="mb-1" href="/users/{gameUser.user.id}">{gameUser.user.nickName}</a>
                  {#if gameUser.status == gameUserStatus.Approved.id}
                  <button type="button" class="btn btn-danger" on:click={() => {removeUserFromGame(gameUser.gameId, gameUser.userId)}}>
                    Удалить игрока {gameUser.userId}. Игра {gameUser.gameId}
                  </button>
                  {:else if gameUser.status == gameUserStatus.Pending.id}
                  <button type="button" class="btn btn-light" on:click={() => {acceptUserToGame(gameUser.gameId, gameUser.userId)}}>
                    Принять заявку игрока {gameUser.userId}. Игра {gameUser.gameId}
                  </button>
                  <button type="button" class="btn btn-danger" on:click={() => {declineUserToGame(gameUser.gameId, gameUser.userId)}}>
                    Отклонить заявку игрока {gameUser.userId}. Игра {gameUser.gameId}
                  </button>
                  {/if}
                </li>
                {/each}
              </ul>
            </li>
            <hr>
            {/each}
          </ul>
          {/if}
        </div>
      </div>
      <!-- <div class="row g-5 my-3">
        <div class="col-md-12">
          <h2 class="text-body-emphasis">Список заявок на хоста </h2>
          <ul class="list-unstyled ps-0">
            {#each [1,2,3,4] as user}
                <li>
                  <a class="mb-1" href="/user/{user}"> users {user} </a>
                  <button type="button" class="btn btn-primary" on:click={() => {acceptHost(user)}}>
                    Принять хоста
                  </button>
                  <button type="button" class="btn btn-danger" on:click={() => {declineHost(user)}}>
                    Удалить хоста
                  </button>
                </li>
            {/each}
          </ul>
        </div>
      </div> -->
      </div>
    </main>
<!--{:catch error}-->
<!--    <p style="color: red">{error.message}</p>-->
<!--{/await}-->


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
                  <label for="name" class="form-label">Имя</label>
                  <input bind:value={userData.name} type="text" class="form-control" id="name" placeholder="" >
                </div>

                <div class="col-12">
                  <label for="description" class="form-label">Описание</label>
                  <textarea bind:value={userData.description} class="form-control" id="description"></textarea>
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
  .user{
      position: relative;
      background-size: cover!important;
      padding: 20px;
      color: #fff;
  }
  a{
    color:#fff
  }
</style>