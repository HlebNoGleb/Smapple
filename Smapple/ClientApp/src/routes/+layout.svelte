<script>
// @ts-nocheck

    import { page } from "$app/stores";
    import { user } from "$lib/user"

    function logout(){
      localStorage.removeItem("user")
      localStorage.removeItem("token")
      location.reload()
    }

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

                <div class="col-auto nav-item">
                  <a class="nav-link" href="/users">Users</a>
                </div>
              </div>
              <div class="nav-item dropdown">
                <a class="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                  {#if user} {user.data.nickName} {:else} Аккаунт {/if}
                </a>
                <ul class="dropdown-menu">
                  {#if user}
                    <li><a class="dropdown-item" href="/account">Аккаунт</a></li>
                    <li><a class="dropdown-item" href="#" on:click={logout}>Выйти</a></li>
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
<div>
  <slot></slot>
</div>
