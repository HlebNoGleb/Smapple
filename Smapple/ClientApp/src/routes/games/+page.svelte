<script>
// @ts-nocheck
  import { page } from "$app/stores";
  import { invalidate } from '$app/navigation';
  import Swal from 'sweetalert2'
  import { user } from "$lib/user"

  export let data;

  let gameTypes = [
    { id: 0, text: `Открытая` },
    { id: 1, text: `Закрытая` },
  ];

  let roles = [
    {id: 0, text: "Хост"},
    {id: 1, text: "Игрок"},
    {id: 2, text: "Админ"}
  ]

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

  function update() {
    location.reload();
  }

  async function submit() {
    const response = await fetch('/api/game', {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${user.token}`
      },
      body: JSON.stringify(addGameForm)
    });

    if (!response.ok) {
      if (response.code == 401) {
        Swal.fire({
          title: 'Ошибка',
          icon: 'error',
          confirmButtonText: 'Ок'
        })
      }
      throw new Error(`HTTP error! status: ${response.status}.`);

    }

    Swal.fire({
      title: 'Отлично',
      text:'Игра создана',
      icon: 'success',
      confirmButtonText: 'Ок'
    })
    update()
  }

  let addGameForm = {
      name: "",
      gameDateTime: null,
      slotsCount: 0,
      address: "",
      type: 0,
      image: ""
  }
</script>

<div class="px-lg-5 px-2">
<div class="container-fluid p-lg-4 p-3 rounded substrate">
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h2>Игры Smapple</h2>
      {#if user?.data.role == roles[0].id}
        <button type="button" class="btn btn-primary " data-bs-toggle="modal" data-bs-target="#addGame">
          Добавить игру
        </button>
      {/if}
    </div>
    <div class="row row-cols-1 row-cols-xxl-5 row-cols-xl-4 row-cols-md-3 row-cols-sm-2 g-3">
        {#each data.games as item}
            <div class="col">
                <div class="card">
                    {#if item.image}
                      <img src="{item.image}" class="card-img-bottom" alt="{item.id} - {item.name}">
                    {/if}
                      <div class="card-body">
                        <div class="my-2">
                            <h5 class="card-title">{item.name}</h5>
                            <a href="/users/{item?.host?.id}" class="card-text">Хост: {item?.host?.nickName}</a>
                            <p>{item.address}</p>
                        </div>
                        <div class="btn-group" role="group" aria-label="Button group with nested dropdown">
                          <a href="/games/{item.id}" class="btn btn-light">Подробнее</a>
                          {#if item?.users && item.users.length > 0}
                            <div class="btn-group" role="group">
                              <button type="button" class="btn btn-light dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">
                                Игроки {item.users.length} / {item.slotsCount}
                              </button>
                                <ul class="dropdown-menu">
                                  {#each item?.gameUsers as gameUser}
                                    {#if gameUser.status == gameUserStatus.Approved.id}
                                      <li><a class="dropdown-item" href="/users/{gameUser.user.id}">{gameUser.user.nickName}</a></li>
                                    {/if}
                                  {/each}
                                </ul>
                            </div>
                          {/if}
                        </div>
                      </div>
                      <div class="card-footer text-muted">
                        <p>Дата игры: {new Date(item.gameDateTime).toLocaleDateString()} {new Date(item.gameDateTime).toLocaleTimeString()}</p>
                      </div>
                </div>
            </div>
        {/each}
    </div>
    {#if user?.data.role == roles[0].id}
      <button type="button" class="btn btn-primary my-3" data-bs-toggle="modal" data-bs-target="#addGame">
        Добавить игру
      </button>
    {/if}
<!-- {:catch error}
    <p style="color: red">{error.message}</p>
{/await} -->
</div>
</div>

<!-- Modal -->
<div class="modal fade" id="addGame" tabindex="-1" aria-labelledby="addGameLabel" aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content substrate">
        <div class="modal-header">
          <h1 class="modal-title fs-5" id="addGameLabel">Добавление игры</h1>
          <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>
        <div class="modal-body">
            <form id="addform" on:submit|preventDefault={submit}>
                <div class="row g-3">
                  <div class="col-sm-6">
                    <label for="title" class="form-label">Название игры</label>
                    <input bind:value={addGameForm.name} type="text" class="form-control" id="title" placeholder="" >
                  </div>

                  <div class="col-sm-6">
                    <label for="datetime" class="form-label">Дата и время</label>
                    <input bind:value={addGameForm.gameDateTime} type="datetime-local" class="form-control" id="datetime" placeholder="" >
                  </div>

                  <div class="col-12">
                    <label for="slots" class="form-label">Кол-во мест</label>
                    <input bind:value={addGameForm.slotsCount} type="number" class="form-control" id="slots" placeholder="" >
                  </div>

                  <div class="col-12">
                    <label for="address" class="form-label">Адрес</label>
                    <input bind:value={addGameForm.address} type="text" class="form-control" id="address" placeholder="" >
                  </div>

                  <div class="col-12">
                    <label for="image" class="form-label">Фон</label>
                    <input bind:value={addGameForm.image} type="text" class="form-control" id="image" placeholder="" >
                  </div>

                  {#if addGameForm.image}
                    <img src="{addGameForm.image}" style="max-width: 100%;" alt=""/>
                  {/if}

                  <div class="col-12">
                    <label for="country" class="form-label">Тип игры</label>
                    <select bind:value={addGameForm.type}>
                      {#each gameTypes as type}
                        <option value={type.id}>
                            {type.text}
                        </option>
                        {/each}
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

<style>
    .card{
        height: 100%;
    }

    .card-body{
        display: flex;
        flex-direction: column;
        align-items: flex-start;
        justify-content: space-between;
    }

    .card a{
      color: #fff;
      text-decoration: none;
    }
</style>