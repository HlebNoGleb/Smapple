export const user = getUser()

function getUser() {
    if (typeof window !== "undefined"){
        const user = localStorage.getItem('user');
        const token = localStorage.getItem('token');
        if (user && token) {
            return {
                data: JSON.parse(user),
                token: token
            }
        }
    }
}