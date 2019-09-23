import React, { Component } from 'react';
import { ButtonGroup, Container, Table } from 'reactstrap';
import "./style.css";
import { bugService } from '../services/BugService';
import { UpdateBugStatus } from './UpdateBugStatus';

export class Bug extends Component {
    static displayName = Bug.name;

    constructor(props) {
        super(props);
        this.state = {
            bug: null,
            new: null
        };

        this.updateBugStatus = this.updateBugStatus.bind(this);
        this.handler = this.handler.bind(this);
    }

    updateBugStatus(bugId, newStatus, comment) {
        bugService.updateBugStatus(bugId, newStatus, comment).then
    }

    id = this.props.match.params.id;

    componentDidMount() {
        bugService.getBug(this.id).then(bug => this.setState({ bug }));
    }

    handler() {
        this.componentDidMount();
    }

    render() {
        const bug = this.state.bug;
        let button, changeLogRows;
        if (bug) {
            if (bug.status === "Created") {
                button = <UpdateBugStatus color="info" action="Open" newStatusId="2" bugId={bug.id} handler={this.handler} />
            }
            else if (bug.status === "Opened") {
                button = <UpdateBugStatus color="success" action="Solve" newStatusId="3" bugId={bug.id} handler={this.handler} />
            }
            else if (bug.status === "Solved") {
                button = <ButtonGroup><UpdateBugStatus color="info" action="Open" newStatusId="2" bugId={bug.id} handler={this.handler} /><UpdateBugStatus color="danger" action="Close" newStatusId="4" bugId={bug.id} handler={this.handler} /></ButtonGroup>
            }
            changeLogRows = bug.bugChangeLog.map((rowData) =>
                <tr>
                    <td>{rowData.date}</td>
                    <td>{rowData.userName}</td>
                    <td>{rowData.newStatus}</td>
                    <td>{rowData.comment}</td>
                </tr>
            );
        }

        return (
            <div>
                {
                    bug &&
                    <Container>
                        <h3>Bug #{bug.id}</h3>
                        <Table>
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Creation Date</th>
                                    <th>Short Description</th>
                                    <th>Importance</th>
                                    <th>Priority</th>
                                    <th>User</th>
                                    <th>Status</th>
                                    {
                                        bug.status !== "Closed" &&
                                        <th>Action</th>
                                    }
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>{bug.id}</td>
                                    <td>{bug.creationDate}</td>
                                    <td>{bug.shortDescription}</td>
                                    <td>{bug.importance}</td>
                                    <td>{bug.priority}</td>
                                    <td>{bug.userName}</td>
                                    <td>{bug.status}</td>
                                    <td>
                                        {button}
                                    </td>
                                </tr>
                            </tbody>
                        </Table>
                        <h5>Full Description</h5>
                        <p>{bug.fullDescription}</p>
                        <h5>Changelog</h5>
                        <Table hover>
                            <thead>
                                <tr>
                                    <th>Date</th>
                                    <th>User</th>
                                    <th>Status</th>
                                    <th>Comment</th>
                                </tr>
                            </thead>
                            <tbody>
                                {changeLogRows}
                            </tbody>
                        </Table>
                    </Container>
                }
            </div>
        )
    };
}