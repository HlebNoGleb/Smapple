export async function load() {
	const res = await fetch('http://localhost:5173/api/game');
    let gamesArray = await res.json();
	return {games:gamesArray}
}