<script>
  let promise = getData();

  async function getData() {
    const res = await fetch(`https://jsonplaceholder.typicode.com/photos?_limit=4`);

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
    alert(JSON.stringify(addGameForm));
    // send to server
    // if 200ok
    update()
  }

  let addGameForm = {
      title: "",
      datetime: null,
      slots: 0,
      address: "",
      type: "0",
      imageUrl: ""
  }
</script>

<div class="container">
  {#await promise}
    <p>...waiting</p>
{:then data}
<div class="my-3">
  <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#exampleModal">
    Добавить игру
  </button>
</div>
    <div class="row row-cols-sm-2 row-cols-md-3 g-4 my-3">
        {#each data as item}
            <div class="col">
                <div class="card">
                    <div class="card-header">
                        Название игры
                      </div>
                      <img src="{item.thumbnailUrl}" class="card-img-bottom" alt="{item.id} - {item.title}">
                      <div class="card-body">
                        <div class="my-2">
                            <h5 class="card-title">{item.title}</h5>
                            <p class="card-text">Имя хоста</p>
                        </div>
                        <a href="/games/{item.id}" class="btn btn-primary">Подробнее</a>
                      </div>
                      <div class="card-footer text-muted">
                        Дата игры
                      </div>
                </div>
            </div>
        {/each}
    </div>
    <div class="my-3">
      <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#exampleModal">
        Добавить игру
      </button>
    </div>
{:catch error}
    <p style="color: red">{error.message}</p>
{/await}
</div>

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
                    <input bind:value={addGameForm.title} type="text" class="form-control" id="title" placeholder="" >
                  </div>

                  <div class="col-sm-6">
                    <label for="datetime" class="form-label">Дата и время</label>
                    <input bind:value={addGameForm.datetime} type="datetime-local" class="form-control" id="datetime" placeholder="" >
                  </div>

                  <div class="col-12">
                    <label for="slots" class="form-label">Кол-во мест</label>
                    <input bind:value={addGameForm.slots} type="number" class="form-control" id="slots" placeholder="" >
                  </div>

                  <div class="col-12">
                    <label for="address" class="form-label">Адрес</label>
                    <input bind:value={addGameForm.address} type="text" class="form-control" id="address" placeholder="" >
                  </div>

                  <div class="col-12">
                    <label for="image" class="form-label">Фон</label>
                    <input bind:value={addGameForm.imageUrl} type="text" class="form-control" id="image" placeholder="" >
                  </div>

                  {#if addGameForm.imageUrl}
                    <img src="{addGameForm.imageUrl}" style="max-width: 100%;" alt=""/>
                  {/if}

                  <div class="col-12">
                    <label for="country" class="form-label">Тип игры</label>
                    <select bind:value={addGameForm.type}>
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
    .card{
        height: 100%;
    }

    .card-body{
        display: flex;
        flex-direction: column;
        align-items: flex-start;
        justify-content: space-between;
    }
</style>