import React, { Component } from 'react';
import { Table } from 'reactstrap';
import { bugService } from '../services/BugService';

const Row = ({ id, creationDate, shortDescription, importance, priority, status, userName }) => (
  <tr>
    <td>{id}</td>
    <td>{creationDate}</td>
    <td>{shortDescription}</td>
    <td>{importance}</td>
    <td>{priority}</td>
    <td>{status}</td>
    <td>{userName}</td>
  </tr>
);

export class Home extends Component {
  static displayName = Home.name;

  constructor(props) {
    super(props);
    this.state = {
      data: null,
    };
  }

  componentDidMount() {
    bugService.getBugs().then(data => this.setState({ data }));
  }

  render() {
    const rows = this.state.data ? this.state.data.map((rowData) => <Row {...rowData} />) : null;
    return (
      <div>
        <h1>All bugs</h1>
        {rows &&
          <Table hover>
            <thead>
              <tr>
                <th>ID</th>
                <th>Creation Date</th>
                <th>Short Description</th>
                <th>Importance</th>
                <th>Priority</th>
                <th>Status</th>
                <th>User</th>
              </tr>
            </thead>
            <tbody>
              {rows}
            </tbody>
          </Table>
        }
      </div>
    );
  }
}