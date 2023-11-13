<script>
    import Swal from 'sweetalert2'
    import { config } from '$lib/config';
    import { goto } from "$app/navigation";


    let signIn = {
        email: "test@test.test",
        password: "1234",
    }

    async function auth() {

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

        await fetch(`${config.mainUrl}/api/login`, {
            method: 'post',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Headers": "Authorization, Origin, X-Requested-With, Content-Type, Accept",
                "Access-Control-Allow-Methods": "GET,PUT,POST,DELETE,PATCH,OPTIONS",
                "Access-Control-Allow-Credentials": "true",
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(signIn)
        })
        .then(function (response) {
            console.log(response);
            if(response.status == 403){
                Swal.fire({
                    title: 'Your account is not verificated',
                    text: "Check your email and verify account",
                    icon: 'warning',
                    confirmButtonText: 'ok'
                })
                
                return null;
            } else {
                return response.json();
            }
        })
        .then(function (result) {
            if (result != null && result.user && result.token) {
                localStorage.setItem('user', JSON.stringify(result.user))
                localStorage.setItem('token', result.token)
                location.href = '/games'
            }
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