<script>
  import {invalidate} from "$app/navigation";

  export let data;
  
  function update() {
      invalidate();
  }
</script>

<div class="container">
<div class="my-3">
  <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#exampleModal">
    Пользователи
  </button>
</div>
    <div class="row row-cols-sm-2 row-cols-md-3 g-4 my-3">
        {#each data.users as item}
            <div class="col">
                <div class="card">
                    <div class="card-header">
                        Имя
                      </div>
                      <img src="{item.nickName}" class="card-img-bottom" alt="{item.id} - {item.nickName}">
                      <div class="card-body">
                        <div class="my-2">
                            <h5 class="card-title">{item.email}</h5>
                            <p class="card-text">Email</p>
                        </div>
                        <a href="/user/{item.id}" class="btn btn-primary">Подробнее</a>
                      </div>
                </div>
            </div>
        {/each}
    </div>
<!--    <div class="my-3">-->
<!--      <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#exampleModal">-->
<!--        Добавить игру-->
<!--      </button>-->
<!--    </div>-->
</div>

<!--&lt;!&ndash; Modal &ndash;&gt;-->
<!--<div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">-->
<!--    <div class="modal-dialog">-->
<!--      <div class="modal-content">-->
<!--        <div class="modal-header">-->
<!--          <h1 class="modal-title fs-5" id="exampleModalLabel">Modal title</h1>-->
<!--          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>-->
<!--        </div>-->
<!--        <div class="modal-body">-->
<!--            <form id="addform" on:submit|preventDefault={submit}>-->
<!--                <div class="row g-3">-->
<!--                  <div class="col-sm-6">-->
<!--                    <label for="title" class="form-label">Название игры</label>-->
<!--                    <input bind:value={addGameForm.title} type="text" class="form-control" id="title" placeholder="" >-->
<!--                  </div>-->

<!--                  <div class="col-sm-6">-->
<!--                    <label for="datetime" class="form-label">Дата и время</label>-->
<!--                    <input bind:value={addGameForm.datetime} type="datetime-local" class="form-control" id="datetime" placeholder="" >-->
<!--                  </div>-->

<!--                  <div class="col-12">-->
<!--                    <label for="slots" class="form-label">Кол-во мест</label>-->
<!--                    <input bind:value={addGameForm.slots} type="number" class="form-control" id="slots" placeholder="" >-->
<!--                  </div>-->

<!--                  <div class="col-12">-->
<!--                    <label for="address" class="form-label">Адрес</label>-->
<!--                    <input bind:value={addGameForm.address} type="text" class="form-control" id="address" placeholder="" >-->
<!--                  </div>-->

<!--                  <div class="col-12">-->
<!--                    <label for="image" class="form-label">Фон</label>-->
<!--                    <input bind:value={addGameForm.imageUrl} type="text" class="form-control" id="image" placeholder="" >-->
<!--                  </div>-->

<!--                  {#if addGameForm.imageUrl}-->
<!--                    <img src="{addGameForm.imageUrl}" style="max-width: 100%;" alt=""/>-->
<!--                  {/if}-->

<!--                  <div class="col-12">-->
<!--                    <label for="country" class="form-label">Тип игры</label>-->
<!--                    <select bind:value={addGameForm.type}>-->
<!--                      <option value="0">Открытая</option>-->
<!--                      <option value="1">Закрытая</option>-->
<!--                    </select>-->
<!--                  </div>-->

<!--                <div class="modal-footer">-->
<!--                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>-->
<!--                    <button type="submit" class="btn btn-primary">Save changes</button>-->
<!--                  </div>-->
<!--              </form>-->
<!--        </div>-->

<!--      </div>-->
<!--    </div>-->
<!--  </div>-->

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