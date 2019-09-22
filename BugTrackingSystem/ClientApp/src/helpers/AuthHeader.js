import { authenticationService } from '../services/AuthenticationService';

export function authHeader() {
    const currentUser = authenticationService.currentUserValue;
    if (currentUser && currentUser.access_token) {
        return { Authorization: `Bearer ${currentUser.access_token}` };
    } else {
        return {};
    }
}