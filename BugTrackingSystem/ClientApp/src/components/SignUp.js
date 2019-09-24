import React, { Component } from 'react';
import { Button, Form, FormGroup, Label, Input } from 'reactstrap';
import "./style.css";
import { authenticationService } from '../services/AuthenticationService';

export class SignUp extends Component {
    static displayName = SignUp.name;

    constructor(props) {
        super(props);

        this.state = {
            username: "",
            email: "",
            password: "",
            confirmPassword: ""
        };
    }

    validateForm() {
        return this.state.username.length > 0 && this.state.email.length > 0 && this.state.password.length > 0 && this.state.password === this.state.confirmPassword;
    }

    handleChange = event => {
        this.setState({
            [event.target.id]: event.target.value
        });
    }

    handleSubmit = event => {
        event.preventDefault();
        this.setState({ isSubmitting: false });
        authenticationService.signUp(this.state.username, this.state.email, this.state.password, this.state.confirmPassword)
            .then(
                user => {
                    const { from } = this.props.location.state || { from: { pathname: "/" } };
                    this.props.history.push(from);
                },
                error => {
                    this.setState({ isSubmitting: false });
                    alert(error);
                }
            );
    }

    render() {
        return (
            <div>
                <Form onSubmit={this.handleSubmit}>
                    <h2>Sign Up</h2>
                    <FormGroup>
                        <Label for="username">Username:</Label>
                        <Input type="text" name="username" id="username" value={this.state.username} onChange={this.handleChange} placeholder="Userame" />
                    </FormGroup>
                    <FormGroup>
                        <Label for="email">Email:</Label>
                        <Input type="email" name="email" id="email" value={this.state.email} onChange={this.handleChange} placeholder=" Email" />
                    </FormGroup>
                    <FormGroup>
                        <Label for="password">Password:</Label>
                        <Input type="password" name="password" id="password" value={this.state.password} onChange={this.handleChange} placeholder="Password" />
                    </FormGroup>
                    <FormGroup>
                        <Label for="confirmPassword">Confirm your password:</Label>
                        <Input type="password" name="confirmPassword" id="confirmPassword" value={this.state.confirmPassword} onChange={this.handleChange} placeholder="Confirm password" />
                    </FormGroup>
                    <Button type="submit" color="primary" disabled={!this.validateForm()}>Sign Up</Button>
                </Form>
            </div>
        )
    }
}