<script>
// @ts-nocheck
    import { page } from "$app/stores";
    let promise = getData();

    console.log($page)

    async function getData() {
        const res = await fetch(`https://jsonplaceholder.typicode.com/users/${$page.params.user}`);

        if (res.ok) {
        	return res.json();
        } else {
        	throw new Error("error");
        }
    }

    function update() {
        promise = getData();
    }

    function submit() {
        alert(JSON.stringify(userData));
        // send to server
        // if 200ok
        update()
    }

    let userData = {
        name: "",
        email:"",
        description:"",
    }
</script>

{#await promise}
    <p>...waiting</p>
{:then data}
    <main>
        <div class="game">
      <div class="container">

            <h1 class="text-body-emphasis">{data.name}</h1>
            <p class="fs-5 col-md-8">{data.email}</p>
            <hr class="col-3 col-md-2 mb-5">
            <div class="my-3">
                <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#exampleModal">
                  Изменить данные
                </button>
              </div>
            </div>
        </div>
        <div class="container">

        <div class="row g-5 my-3">
          <div class="col-md-6">
            <h2 class="text-body-emphasis">Список игр в которых он учавствовал</h2>
            <ul class="list-unstyled ps-0">
              {#each [1,2,3,4] as item}
              <li>
                <a class="mb-1" href="/games/{item}">
                  Game {item}
                </a>
              </li>
              {/each}
            </ul>
          </div>
          <div class="col-md-6">
            <h2 class="text-body-emphasis">Список игр в которые он подал заявки</h2>
            <ul class="list-unstyled ps-0">
              {#each [1,2,3,4] as item}
              <li>
                <a class="mb-1" href="/games/{item}">
                  Game {item}
                </a>
              </li>
              {/each}
            </ul>
          </div>
        </div>
        </div>
      </main>
{:catch error}
    <p style="color: red">{error.message}</p>
{/await}


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
    .game{
        position: relative;
        background-size: cover!important;
        padding: 20px;
        color: #000;
    }
</style>