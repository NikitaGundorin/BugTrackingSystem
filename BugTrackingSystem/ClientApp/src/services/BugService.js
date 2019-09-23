import { apiUrl } from '../helpers/apiUrl';
import { authHeader } from '../helpers/AuthHeader';
import { handleResponse } from '../helpers/HandleResponse';

export const bugService = {
    getBugs,
    getBug,
    addBug,
    updateBugStatus,
    getParams
};

function getBugs() {
    const requestOptions = { method: 'GET', headers: authHeader() };
    return fetch(`${apiUrl}/home/index`, requestOptions).then(handleResponse);
}

function getBug(id){
    const requestOptions = { method: 'GET', headers: authHeader() };
    return fetch(`${apiUrl}/home/bug/${id}`, requestOptions).then(handleResponse);
}

function addBug(shortDescription, fullDescription, priorityId, importanceId) {

    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', 'Authorization': `${authHeader().Authorization}` },
        body: JSON.stringify({ shortDescription, fullDescription, priorityId, importanceId })
    };

    return fetch(`${apiUrl}/home/addbug`, requestOptions).then(handleResponse)
}

function updateBugStatus(bugId, newStatusId, comment) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', 'Authorization': `${authHeader().Authorization}` },
        body: JSON.stringify({ bugId, newStatusId, comment })
    };

    return fetch(`${apiUrl}/home/updatebugstatus`, requestOptions).then(handleResponse)
}

function getParams() {
    const requestOptions = { method: 'GET', headers: authHeader() };
    return fetch(`${apiUrl}/home/addbug`, requestOptions).then(handleResponse);
}