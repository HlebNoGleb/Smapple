export async function load({ fetch }) {
	const res = await fetch('https://jsonplaceholder.typicode.com/photos?_limit=10');
    let gamesArray = await res.json();
	return {games:gamesArray}
}