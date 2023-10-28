export async function load({ fetch }) {
	const res = await fetch('/api/game');
    console.log(res);
    let gamesArray = await res.text();
	return {games:gamesArray}
}