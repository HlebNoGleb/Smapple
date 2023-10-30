
export async function load({params}) {
    if (typeof window !== "undefined") {

        const token = localStorage.getItem('token');

        const res = await fetch(`/api/user/me`, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`
            },
        })
            .then(function (response) {
                return response.json();
            });

        return {me: res}
    }
}