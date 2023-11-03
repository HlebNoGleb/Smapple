// @ts-nocheck

export async function load({params}) {
    return {
		me: await getMe()
	};
}

async function getMe(){
    if (typeof window == "undefined") {
        return null
    }

    const token = localStorage.getItem('token');
    const res = await fetch(`/api/user/me`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        },
    });

    let me = await res.json();
	return me;
}