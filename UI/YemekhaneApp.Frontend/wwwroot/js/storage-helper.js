// Storage helper functions
export function getToken() {
    try {
        return localStorage.getItem('jwtToken') || '';
    } catch (error) {
        console.error('Error getting token:', error);
        return '';
    }
}

export function setToken(token) {
    try {
        localStorage.setItem('jwtToken', token);
        return true;
    } catch (error) {
        console.error('Error setting token:', error);
        return false;
    }
}

export function removeToken() {
    try {
        localStorage.removeItem('jwtToken');
        return true;
    } catch (error) {
        console.error('Error removing token:', error);
        return false;
    }
}