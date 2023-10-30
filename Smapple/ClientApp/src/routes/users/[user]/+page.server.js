export async function load({params}) {
    const res = await fetch(`http://localhost:5173/api/user/${params.user}`);
    let user = await res.json();
	return {user:user}
}