// @ts-nocheck
export const prerender = true;

export async function load({params}) {
    return {
		game: await getGame(params.game)
	};
}

async function getGame(id){
    const res = await fetch(`http://localhost:5173/api/game/${id}`);
    let game = await res.json();
    console.log(game)
	return game;
}