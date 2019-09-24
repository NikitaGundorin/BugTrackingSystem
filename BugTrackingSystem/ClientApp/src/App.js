import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, Button } from 'reactstrap';
import { createBrowserHistory } from 'history';
import { Route, Router, Link } from 'react-router-dom';
import { Home } from './components/Home';
import { SignIn } from './components/SignIn';
import { SignUp } from './components/SignUp';
import { AddBug } from './components/AddBug';
import { Bug } from './components/Bug';
import { PrivateRoute } from './components/PrivateRoute';
import { authenticationService } from './services/AuthenticationService';

export const history = createBrowserHistory();

export default class App extends Component {
    static displayName = App.name;

    constructor(props) {
        super(props);

        this.state = {
            collapsed: true,
            currentUser: null
        };
        this.toggleNavbar = this.toggleNavbar.bind(this);
    }

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed
        });
    }

    componentDidMount() {
        authenticationService.currentUser.subscribe(x => this.setState({ currentUser: x }));
    }

    logout() {
        authenticationService.logout();
        history.push('/sign-in');
    }

    render() {
        const { currentUser } = this.state;
        return (
            <Router history={history}>
                <div>
                    <header>
                        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
                            <Container>
                                <NavbarBrand tag={Link} to="/">BugTrackingSystem</NavbarBrand>
                                <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
                                <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
                                    {currentUser &&
                                        <ul className="navbar-nav flex-grow">
                                            <Button tag={Link} color="primary" className="navitem" to="/add-bug" style={{ marginRight: "5px" }}>Add Bug</Button>
                                            <NavItem>
                                                <NavLink tag={Link} className="text-dark navitem" to="/">All bugs</NavLink>
                                            </NavItem>
                                            <NavItem>
                                                <NavLink onClick={this.logout} className="text-dark navitem navlink" style={{ cursor: "pointer" }}>Logout</NavLink>
                                            </NavItem>
                                        </ul>
                                    }
                                    {!currentUser &&
                                        <ul className="navbar-nav flex-grow">
                                            <NavItem>
                                                <NavLink tag={Link} className="text-dark navitem" to="/sign-in">Sign In</NavLink>
                                            </NavItem>
                                            <Button tag={Link} className="navitem" color="primary" to="/sign-up" style={{ marginLeft: "5px" }}>Sign Up</Button>
                                        </ul>
                                    }
                                </Collapse>
                            </Container>
                        </Navbar>
                    </header>
                    <Container>
                        <PrivateRoute exact path="/" component={Home} />
                        <Route path="/sign-in" component={SignIn} />
                        <Route path="/sign-up" component={SignUp} />
                        <PrivateRoute path="/add-bug" component={AddBug} />
                        <PrivateRoute path="/bug/:id" component={Bug} />
                    </Container>
                </div>
            </Router>
        );
    }
}