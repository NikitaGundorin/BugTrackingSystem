import React, { Component } from 'react';
import { ButtonGroup, Container, Table, Spinner } from 'reactstrap';
import "./style.css";
import { bugService } from '../services/BugService';
import { UpdateBugStatus } from './UpdateBugStatus';
import { history } from '../App'
import { UpdateBug } from './UpdateBug';
import { authenticationService } from '../services/AuthenticationService';

export class Bug extends Component {
    static displayName = Bug.name;

    constructor(props) {
        super(props);
        this.state = {
            bug: null,
            new: null,
            isLoad: false
        };

        this.updateBugStatus = this.updateBugStatus.bind(this);
        this.handler = this.handler.bind(this);
    }

    updateBugStatus(bugId, newStatus, comment) {
        bugService.updateBugStatus(bugId, newStatus, comment)
    }

    id = this.props.match.params.id;

    componentDidMount() {
        this.setState({ isLoad: false });
        bugService.getBug(this.id).then(bug => this.setState({ bug, isLoad: true }));
    }

    handler() {
        this.componentDidMount();
    }

    render() {
        const bug = this.state.bug;
        const currentUser = authenticationService.currentUserValue;
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
                <a href="#" onClick={() => history.push('/')}>All bugs</a>
                {
                    this.state.isLoad ?
                        <Container>
                            <div className="infoBlock">
                                <h3 style={{ display: "inline-block" }}>Bug #{bug.id}</h3>
                                {
                                    currentUser.role === "admin" &&
                                    <UpdateBug id={bug.id} shortDescription={bug.shortDescription} fullDescription={bug.fullDescription} importance={bug.importance} priority={bug.priority} handler={this.handler} />
                                }
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
                                            {
                                                bug.status !== "Closed" &&
                                                <td>
                                                    {button}
                                                </td>
                                            }
                                        </tr>
                                    </tbody>
                                </Table>
                            </div>
                            <div className="infoBlock">

                                <h5>Full Description</h5>
                                <p>{bug.fullDescription}</p>
                            </div>
                            <div className="infoBlock">
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
                            </div>
                        </Container>
                        :
                        <div style={{ display: "flex", justifyContent: "center" }} >
                            <Spinner color="primary" />
                        </div>
                }
            </div>
        )
    };
}