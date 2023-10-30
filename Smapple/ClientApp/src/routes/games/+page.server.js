export const prerender = false;

export async function load() {
	const res = await fetch('http://localhost:5125/api/game');
    let gamesArray = await res.json();
	return {games:gamesArray}
}