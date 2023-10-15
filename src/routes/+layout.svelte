<script>
    import { page } from "$app/stores";

</script>
    <nav class="navbar navbar-expand-sm navbar-dark bg-dark" aria-label="Third navbar example">
        <div class="container-fluid">
          <a class="navbar-brand" href="/">Smapple</a>
          <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarsExample03" aria-controls="navbarsExample03" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
          </button>

          <div class="collapse navbar-collapse" id="navbarsExample03">
            <ul class="navbar-nav me-auto mb-2 mb-sm-0">
              <li class="nav-item">
                <a class="nav-link" href="/">Account</a>
              </li>
              <li class="nav-item">
                <a class="nav-link" href="/calendar">Calendar</a>
              </li>
              <li class="nav-item">
                <a class="nav-link" href="/game">Game</a>
              </li>
              <div class="nav-item dropdown">
                <a class="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                  Auth
                </a>
                <ul class="dropdown-menu">
                  <li><a class="dropdown-item" href="/signin">Sign In</a></li>
                  <li><a class="dropdown-item" href="/signup">Sign Up</a></li>
                  <li><a class="dropdown-item" href="#">Logout</a></li>
                </ul>
              </div>
            </ul>
          </div>
        </div>
      </nav>

<slot></slot>