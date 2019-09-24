import { apiUrl } from '../helpers/apiUrl';
import { authHeader } from '../helpers/AuthHeader';
import { handleResponse } from '../helpers/HandleResponse';

export const bugService = {
    getBugs,
    getBug,
    addBug,
    updateBugStatus,
    updateBug,
    getParams
};

function getBugs(sortOrder, page, pageSize) {
    const requestOptions = { method: 'GET', headers: authHeader() };
    return fetch(`${apiUrl}/home/index?sortOrder=${sortOrder}&page=${page}&pageSize=${pageSize}`, requestOptions).then(handleResponse);
}

function getBug(id) {
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

function updateBug(id, shortDescription, fullDescription, importanceId, priorityId) {
    console.log(importanceId);
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', 'Authorization': `${authHeader().Authorization}` },
        body: JSON.stringify({ id, shortDescription, fullDescription, importanceId, priorityId })
    };

    return fetch(`${apiUrl}/home/updatebug`, requestOptions).then(handleResponse)
}

function getParams() {
    const requestOptions = { method: 'GET', headers: authHeader() };
    return fetch(`${apiUrl}/home/addbug`, requestOptions).then(handleResponse);
}