<script>
    import { goto } from "$app/navigation";
    import Swal from 'sweetalert2'
    import { config } from '$lib/config';

    let signUp = {
        email: "test@test.test",
        password: "1234",
        nickname: "111111"
    }

    async function auth() {

        let validation = true;

        Object.values(signUp).forEach(item => {
            if (!item) {
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

        await fetch(`${config.mainUrl}/api/register`, {
                method: 'post',
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Headers": "Authorization, Origin, X-Requested-With, Content-Type, Accept",
                    "Access-Control-Allow-Methods": "GET,PUT,POST,DELETE,PATCH,OPTIONS",
                    "Access-Control-Allow-Credentials": "true",
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(signUp)
            })
            .then(function (response) {
                return response.json();
            })
            .then(function (result) {
                console.log(result);
            })
            .catch(function (error) {
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

<div class="form-signup__container">
        <main class="form-signup w-100 m-auto text-center">
            <h1 class="h3 mb-3 fw-normal">Please sign up</h1>
            <form on:submit|preventDefault={auth}>
                <div class="form-floating">
                    <input bind:value={signUp.nickname} type="text" class="form-control" id="login" placeholder="">
                    <label for="login">Ник</label>
                </div>
                <div class="form-floating">
                    <input bind:value={signUp.email} type="email" class="form-control" id="email" placeholder="name@example.com">
                    <label for="email">Email</label>
                </div>
                <div class="form-floating">
                    <input bind:value={signUp.password} type="password" class="form-control" id="password" placeholder="Password">
                    <label for="password">Пароль</label>
                </div>
                <button class="w-100 btn btn-lg btn-primary" type="submit">Sign Up</button>
            </form>
        </main>
</div>

<style>
    .form-signup__container{
        display: flex;
        align-items: center;
    }

    .form-signup {
        max-width: 450px;
        padding: 15px;
    }

    .form-signup .form-floating:focus-within {
        z-index: 2;
    }

    .form-floating input{
        border-top-left-radius: 0;
        border-top-right-radius: 0;
        border-bottom-right-radius: 0;
        border-bottom-left-radius: 0;
    }

    .form-floating:nth-of-type(1) input {
        margin-bottom: -1px;
        border-top-left-radius: 6px;
        border-top-right-radius: 6px;
    }

    .form-floating:nth-last-of-type(1) input {
        margin-bottom: 10px;
        border-bottom-right-radius: 6px;
        border-bottom-left-radius: 6px;
    }
</style>