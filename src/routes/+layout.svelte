<script>
    import { page } from "$app/stores";

</script>

{#if $page.route.id !== "/"}
    <nav class="navbar navbar-expand-sm navbar-dark bg-dark" aria-label="Third navbar example">
        <div class="container-fluid">
          <a class="navbar-brand" href="/">Smapple</a>
          <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarsExample03" aria-controls="navbarsExample03" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
          </button>

          <div class="collapse navbar-collapse" id="navbarsExample03">
            <ul class="navbar-nav me-auto mb-2 mb-sm-0">
              <li class="nav-item">
                <a class="nav-link" href="/">Home</a>
              </li>
              <li class="nav-item">
                <a class="nav-link" href="/calendar">Calendar</a>
              </li>
              <li class="nav-item">
                <a class="nav-link" href="/game">Game</a>
              </li>
              <li class="nav-item">
                <a class="nav-link">Logout</a>
              </li>
            </ul>
          </div>
        </div>
      </nav>
{:else}
    <nav class="navbar navbar-expand-sm navbar-dark bg-dark" aria-label="Third navbar example">
        <div class="container-fluid">
        <a class="navbar-brand" href="/">Smapple</a>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarsExample03" aria-controls="navbarsExample03" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>

        <div class="collapse navbar-collapse" id="navbarsExample03">
            <ul class="navbar-nav me-auto mb-2 mb-sm-0">
            <li class="nav-item">
                <a class="nav-link" href="/">Home</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="/signin">Sigh in</a>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="/signup">Sign up</a>
            </li>
            </ul>
        </div>
        </div>
    </nav>
{/if}

<slot></slot>