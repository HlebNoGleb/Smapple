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
    <div>Ваш аккаунт успешно верифицирован</div>
  {:else}
    <div>Ошибка</div>
  {/if}
</main>

<style>
  .user{
      position: relative;
      background-size: cover!important;
      padding: 20px;
      color: #fff;
  }
  a{
    color:#fff
  }
</style>