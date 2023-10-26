<script>
    import Swal from 'sweetalert2'
    import { config } from '$lib/config';

    let signIn = {
        email: "",
        password: "",
        remember: false,
    }

    async function auth() {

        // add jwt logic

        let validation = true;

        Object.values(signIn).forEach(item => {
            if (!item){
                Swal.fire({
                    title: 'Oups...',
                    text: 'All fields must be filled in',
                    icon: 'error',
                    confirmButtonText: 'ok'
                });

                validation = false;
                return;
            }
        });

        if (!validation) {
            return;
        }

        await fetch(`${config.mainUrl}/signin`, {
            method: 'post',
            headers: {
                'Accept': 'application/json, text/plain, */*',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(signIn)
        })
        .then(function (response) {
            return response.json();
        })
        .then(function (result) {

        })
        .catch (function (error) {
            console.log('Request failed', error);
            Swal.fire({
                title: 'Oups...',
                text: 'Something went wrong',
                icon: 'error',
                confirmButtonText: 'ok'
            })
        });
    }
</script>
<div class="form-signin__container">
    <main class="form-signin w-100 m-auto text-center">
        <form on:submit|preventDefault={auth}>
            <h1 class="h3 mb-3 fw-normal">Please sign in</h1>
            <div class="form-floating">
                <input type="email" bind:value={signIn.email} class="form-control" id="floatingInput" placeholder="name@example.com">
                <label for="floatingInput">Email address</label>
            </div>
            <div class="form-floating">
                <input type="password" bind:value={signIn.password} class="form-control" id="floatingPassword" placeholder="Password">
                <label for="floatingPassword">Password</label>
            </div>

            <div class="checkbox mb-3">
                <label>
                <input type="checkbox" bind:value={signIn.remember}> Remember me
                </label>
            </div>
            <button class="w-100 btn btn-lg btn-primary" type="submit">Sign in</button>
        </form>
    </main>
</div>

<style>
    .form-signin__container{
        display: flex;
        align-items: center;
    }

    .form-signin {
        max-width: 450px;
        padding: 15px;
    }

    .form-signin .form-floating:focus-within {
        z-index: 2;
    }

    .form-signin input[type="email"] {
        margin-bottom: -1px;
        border-bottom-right-radius: 0;
        border-bottom-left-radius: 0;
    }

    .form-signin input[type="password"] {
        margin-bottom: 10px;
        border-top-left-radius: 0;
        border-top-right-radius: 0;
    }
</style>