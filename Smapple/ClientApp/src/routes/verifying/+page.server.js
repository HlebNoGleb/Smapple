export async function load({  url }) {
	let code = url.searchParams.get('code');
	const res = await fetch(`http://localhost:5125/api/verify?code=${code}`);
	return {isVeryfied:res.ok}
}