export async function load({params}) {
    const res = await fetch(`http://localhost:5173/api/game/${params.game}`);
    let game = await res.json();
    console.log(game)
	return {game:game}
}