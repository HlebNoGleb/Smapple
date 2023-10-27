<script>
    import { goto } from "$app/navigation";
    import { onMount } from 'svelte';

    onMount(() => {
        if (false) { // check auth
            goto('/games');
        }
    });

</script>


<h1>Smapple</h1>