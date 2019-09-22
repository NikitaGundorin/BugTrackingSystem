import { apiUrl } from '../helpers/apiUrl';
import { authHeader } from '../helpers/AuthHeader';
import { handleResponse } from '../helpers/HandleResponse';

export const bugService = {
    getBugs,
    addBug,
    getParams
};

function getBugs() {
    const requestOptions = { method: 'GET', headers: authHeader() };
    return fetch(`${apiUrl}/home/index`, requestOptions).then(handleResponse);
}

function addBug(shortDescription, fullDescription, priorityId, importanceId) {
    const tokenHeader = authHeader();
    console.log(tokenHeader);

    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', 'Authorization': `${authHeader().Authorization}` },
        body: JSON.stringify({ shortDescription, fullDescription, priorityId, importanceId })
    };

    return fetch(`${apiUrl}/home/addbug`, requestOptions).then(handleResponse)
}

function getParams() {
    const requestOptions = { method: 'GET', headers: authHeader() };
    return fetch(`${apiUrl}/home/addbug`, requestOptions).then(handleResponse);
}