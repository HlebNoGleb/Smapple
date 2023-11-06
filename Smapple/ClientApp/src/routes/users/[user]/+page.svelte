<script lang="ts">
// @ts-nocheck
    import { page } from "$app/stores";
    import { invalidate } from '$app/navigation';
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
      history: [],
      hosted: [],
    }

    function init(params) {
      userModel.history = data.user.gameUsers ? data?.user?.gameUsers.filter(x=>x.status == gameUserStatus.Approved.id) : [];
      userModel.hosted = data?.user?.hostedGames || [];
    }

    init();

    function submit() {
        alert(JSON.stringify(userData));
    }

    let userData = {
        name: "",
        email:"",
        description:"",
        points: 100,
    }

    function addToGame(game, user) {
      alert(`добавить пользователя ${user} в игру ${game}`)
    }

    function removeFromGame(game, user) {
      alert(`удалить пользователя ${user} из игры ${game}`)
    }

    function removeFromGameApplication(game, user) {
      alert(`удалить заявку пользователя ${user} из игры ${game}`)
    }

    function acceptHost(user) {
      alert(`принять пользователя ${user} как хоста`)
    }

    function declineHost(user) {
      alert(`отказать пользователю ${user} как хосту`)
    }


</script>

<!--{#await promise}-->
<!--    <p>...waiting</p>-->
<!--{:then data}-->
    <main>
      <div class="user">
        <div class="container">
          <h1 class="text-body-emphasis">{data.user.nickName}</h1>
          <h1 class="text-body-emphasis">Очки: 100</h1>
          <p class="fs-5 col-md-8">{data.user.email}</p>
          <hr class="col-3 col-md-2 mb-5">
          {#if data.user.id == user?.data?.id}
            <div class="my-3">
              <button type="button" class="btn btn-light" data-bs-toggle="modal" data-bs-target="#exampleModal">
                Изменить данные
              </button>
            </div>
          {/if}
        </div>
      </div>
      <div class="container">

        <div class="row g-5 my-3">
          {#if userModel.history.length > 0}
          <div class="col-md-6">
            <h2 class="text-body-emphasis">История игр</h2>
            <ul class="list-unstyled ps-0">
              {#each userModel.history as history}
                <li>
                  <a class="mb-1" href="/games/{history.game.id}">
                    {history.game.name}
                  </a>
                  <span>Очки: {history.userScore}</span>
                </li>
              {/each}
            </ul>
          </div>
          {/if}
          <!-- <div class="col-md-6">
            <h2 class="text-body-emphasis">Заявки в игры</h2>
            <ul class="list-unstyled ps-0">
              {#each data.user?.gameUsers as gameUser}
                {#if gameUser.status == gameUserStatus.Pending.id}
                <li>
                  <a class="mb-1" href="/games/{gameUser.game.id}">
                    {gameUser.game.name}
                  </a>
                </li>
                {/if}
              {/each}
            </ul>
          </div>
        </div> -->
        <div class="row g-5 my-3">
          <div class="col-md-12">
            {#if userModel.hosted.length > 0}
            <h2 class="text-body-emphasis">История ведения игр</h2>
            <ul class="list-unstyled ps-0">
              {#each userModel.hosted as hosted}
              <li>
                <a class="mb-1" href="/games/{hosted.id}">
                  {hosted.name}
                </a>
                <!-- <ul class="list-unstyled ps-0">
                  {#each [1,2,3,4] as user}
                  <li>
                    <a class="mb-1" href="/user/{user}"> users {user} </a>
                    <button type="button" class="btn btn-primary" on:click={() => {addToGame(game, user)}}>
                      Принять игрока
                    </button>
                    <button type="button" class="btn btn-danger" on:click={() => {removeFromGame(game, user)}}>
                      Удалить игрока
                    </button>
                  </li>
                  {/each}
                </ul> -->
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

                  <div class="col-sm-6">
                    <label for="email" class="form-label">Email</label>
                    <input bind:value={userData.email} type="email" class="form-control" id="email" placeholder="" >
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