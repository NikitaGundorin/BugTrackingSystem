import React, { Component } from 'react';
import { Table, Button, Pagination, PaginationItem, PaginationLink, Spinner } from 'reactstrap';
import { bugService } from '../services/BugService';
import { history } from '../App'
import "./style.css";

const Row = ({ id, creationDate, shortDescription, importance, priority, status, userName }) => (
  <tr style={{ varticalAlign: 'center' }}>
    <td>{id}</td>
    <td>{creationDate}</td>
    <td>{shortDescription}</td>
    <td>{importance}</td>
    <td>{priority}</td>
    <td>{userName}</td>
    <td>{status}</td>
    <td><Button outline block color="primary" size="sm" style={{ lineHeight: "1rem" }} onClick={() => { openBug(id) }}>Edit</Button></td>
  </tr>
);

function openBug(id) {
  history.push('/bug/' + id);
};

export class Home extends Component {
  static displayName = Home.name;

  constructor(props) {
    super(props);
    this.state = {
      isLoad: false,
      data: {
        bugs: [],
        pages: {
          pageNumber: 0,
          totalPages: 0
        }
      },
      sorted: "Id",
      order: "Asc",
      pageSize: 10
    };
  }

  sort(sorted) {
    let sortOrder;
    if (this.state.sorted === sorted) {
      if (this.state.order === "Desc") {
        sortOrder = sorted + "Asc";
        this.setState({ order: "Asc" });
      }
      else {
        sortOrder = sorted + "Desc";
        this.setState({ order: "Desc" });
      }
    }
    else {
      sortOrder = sorted + "Asc";
      this.setState({ sorted: sorted, order: "Asc" });
    }
    this.componentDidMount(sortOrder);
  }

  componentDidMount(sortOrder = "IdAsc", page = 1) {
    bugService.getBugs(sortOrder, page, this.state.pageSize).then(data => this.setState({ data, isLoad: true }));
  }

  render() {
    const rows = this.state.data.bugs.map((rowData) => <Row {...rowData} key={rowData.id} />);
    const arrow = this.state.order === "Asc" ? <span>▴</span> : <span>▾</span>;

    let items = [];
    const pages = this.state.data.pages;
    for (let number = 1; number <= pages.totalPages; number++) {
      items.push(
        <PaginationItem key={number} active={number === pages.pageNumber}>
          <PaginationLink onClick={() => this.componentDidMount(this.state.sorted + this.state.order, number)}>{number}</PaginationLink>
        </PaginationItem>
      );
    }

    const paginationBasic = (
      <Pagination style={{ justifyContent: "center" }} maxSize="1">{items}</Pagination>
    );

    return (
      <div className="infoBlock">
        <h1>All bugs</h1>
        {this.state.isLoad ?
          <Table hover>
            <thead>
              <tr>
                <th onClick={() => this.sort("Id")}>ID {this.state.sorted === "Id" && arrow} </th>
                <th onClick={() => this.sort("CreationDate")}>Creation Date {this.state.sorted === "CreationDate" && arrow} </th>
                <th onClick={() => this.sort("ShortDescription")}>Short Description {this.state.sorted === "ShortDescription" && arrow} </th>
                <th onClick={() => this.sort("Importance")}>Importance {this.state.sorted === "Importance" && arrow} </th>
                <th onClick={() => this.sort("Priority")}>Priority {this.state.sorted === "Priority" && arrow} </th>
                <th onClick={() => this.sort("UserName")}>User {this.state.sorted === "UserName" && arrow} </th>
                <th onClick={() => this.sort("Status")}>Status {this.state.sorted === "Status" && arrow} </th>
                <th>Edit</th>
              </tr>
            </thead>
            <tbody>
              {rows}
            </tbody>
          </Table>
          :
          <div style={{ display: "flex", justifyContent: "center" }} >
            <Spinner color="primary" />
          </div>
        }
        {paginationBasic}
      </div>
    );
  }
}
