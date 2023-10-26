<svelte:head>
    <script src="https://api-maps.yandex.ru/2.1/?lang=ru_RU" type="text/javascript"></script>
</svelte:head>

<script>
// @ts-nocheck

    import { page } from '$app/stores';
    let promise = getData();

    async function getData() {
        return null
        // const res = await fetch(`https://jsonplaceholder.typicode.com/users/${$page.params.slug}`);

        // if (res.ok) {
        // 	return res.json();
        // } else {
        // 	throw new Error("error");
        // }
    }

    function update() {
        promise = getData();
    }

    function setYandexMaps(node) {
        ymaps.ready(init);
        const lat = node.getAttribute("data-lat")
        const long = node.getAttribute("data-long")
        const name = node.getAttribute("data-name")


            function init () {
                var myMap = new ymaps.Map('map',
                {
                    center: [lat, long],
                    zoom: 15,
                    controls: ["zoomControl", "fullscreenControl"]
                });

                var pin = new ymaps.Placemark(
                [parseFloat(lat), parseFloat(long)],
                {
                    iconCaption: name
                },
                {
                    openBalloonOnClick: false,
                    preset: 'islands#blueCircleDotIcon'
                });

                myMap.geoObjects.add(pin);
            }
    }
</script>

{#await promise}
    <p>...waiting</p>
{:then data}
    <main>
        <div class="game" style="background:linear-gradient(0deg, rgba(0,0,0,.5), rgba(0,0,0,.5)), url(https://pbs.twimg.com/media/FQ-TyYnXIAcR0uu.jpg:medium);">
      <div class="container">

            <h1 class="text-body-emphasis">Название игры</h1>
            <p class="fs-5 col-md-8">Описание игры (если есть - вывести, если нет - добавить)</p>
            <p class="fs-5 col-md-8">{new Date().toLocaleDateString()} - {new Date().toLocaleTimeString()}</p>
            <div class="mb-5">
                <a class="btn btn-primary btn-lg px-4">Присоединиться (closed game)</a>
                <a class="btn btn-primary btn-lg px-4">Присоединиться (open game)</a>
            </div>
            <hr class="col-3 col-md-2 mb-5">
            </div>
        </div>
        <div class="container">

        <div class="row g-5 my-3">
          <div class="col-md-6">
            <h2 class="text-body-emphasis">Список игроков</h2>
            <ul class="list-unstyled ps-0">
              {#each [1,2,3,4] as item}
              <li>
                <a class="mb-1" rel="noopener" target="_blank">
                  Nickname {item}
                </a>
              </li>
              {/each}
            </ul>
          </div>
          <div class="col-md-6">
            <div id="map" style="width: 100%; height: 400px" data-lat="57" data-long="22" data-name="название игры" use:setYandexMaps></div>
          </div>
        </div>
        </div>
      </main>
{:catch error}
    <p style="color: red">{error.message}</p>
{/await}

<style>
    .game{
        position: relative;
        background-size: cover!important;
        padding: 20px;
        color: #fff;
    }
</style>