<script lang="ts">
// @ts-ignore
    import Swal from 'sweetalert2'
    import { onMount } from 'svelte';
    import { user } from "$lib/user"
    import { gameStatus, gameUserStatus } from '$lib/enums.js';
    export let data;
</script>

<main>
  {#if data.isVeryfied}
    <div class="text-center">
      <h1 class="display-6 fw-bold text-body-emphasis">✅ Ваш аккаунт успешно верифицирован</h1>
      <a href="/signin" type="button" class="btn btn-light btn-lg px-4 gap-3">Войти</a>
    </div>
  {:else}
    <div class="text-center">
      <h1 class="display-6 fw-bold text-body-emphasis">❌ Ошибка</h1>
    </div>
  {/if}
</main>

<style>
  .user{
      position: relative;
      background-size: cover!important;
      padding: 20px;
      color: #fff;
  }
</style>