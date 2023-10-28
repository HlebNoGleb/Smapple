export async function load({ fetch }) {
	const res = await fetch('/api/game');
    let gamesArray = await res.json();
    console.log(gamesArray)
	return {games:gamesArray}
}

export const _test = {
	login: async () => {
        console.log(1234567)
    },
	register: async () => {
		// TODO register the user
	}
};