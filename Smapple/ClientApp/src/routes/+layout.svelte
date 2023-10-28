<script>
// @ts-nocheck

    import { page } from "$app/stores";
    let user = {
      name: "User 1",
      id: 1,
    }

    // user = null

</script>
    <nav class="navbar navbar-expand-sm navbar-dark bg-dark px-lg-5 px-2 py-3" aria-label="Third navbar example">
        <div class="container-fluid">
          <a class="navbar-brand" href="/">Smapple</a>
          <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarsExample03" aria-controls="navbarsExample03" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
          </button>

          <div class="collapse navbar-collapse" id="navbarsExample03">
            <div class="navbar-nav me-auto mb-2 mb-sm-0" style="width: 100%;">
              <div class="row" style="width: 100%;">

                <div class="col-auto nav-item">
                  <a class="nav-link" href="/games">Games</a>
                </div>
              </div>
              <div class="nav-item dropdown">
                <a class="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                  {#if user} {user.name} {:else} Аккаунт {/if}
                </a>
                <ul class="dropdown-menu">

                  {#if user}
                    <li><a class="dropdown-item" href="/users/{user.id}">Аккаунт</a></li>
                    <li><a class="dropdown-item" href="#">Выйти</a></li>
                  {:else}
                    <li><a class="dropdown-item" href="/signin">Авторизация</a></li>
                    <li><a class="dropdown-item" href="/signup">Регистрация</a></li>
                  {/if}
                </ul>
              </div>
            </div>
          </div>
        </div>
      </nav>

      <slot></slot>