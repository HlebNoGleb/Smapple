export async function load() {
	const res = await fetch('http://localhost:5173/api/user');
	let usersArray = await res.json();	
	return {users:usersArray}
}