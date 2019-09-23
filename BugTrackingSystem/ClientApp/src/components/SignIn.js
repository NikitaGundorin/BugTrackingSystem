import React, { Component } from 'react';
import { Button, Form, FormGroup, Label, Input } from 'reactstrap';
import "./style.css";
import { authenticationService } from '../services/AuthenticationService';

export class SignIn extends Component {
    static displayName = SignIn.name;

    constructor(props) {
        super(props);

        this.state = {
            username: "",
            password: "",
            isSubmitting: false
        };

        if (authenticationService.currentUserValue) { 
            this.props.history.push('/');
        }
    }

    validateForm() {
        return this.state.username.length > 0 && this.state.password.length > 0;
    }
    
    handleChange = event => {
        this.setState({
            [event.target.id]: event.target.value
        });
    }
    
    handleSubmit = event => {
        event.preventDefault();
        this.setState({isSubmitting: false});
        authenticationService.login(this.state.username, this.state.password)
        .then(
            user => {
                const { from } = this.props.location.state || { from: { pathname: "/" } };
                this.props.history.push(from);
            },
            error => {
                this.setState({isSubmitting: false});
                alert(error);
            }
        );
    }
    
    render() {
        return (
            <div>
                <Form onSubmit={this.handleSubmit}>
                    <h2>Sign In</h2>
                    <FormGroup>
                        <Label for="username">Username or Email:</Label>
                        <Input type="text" name="username" id="username" value={this.state.username} onChange={this.handleChange} placeholder="Userame or Email" />
                    </FormGroup>
                    <FormGroup>
                        <Label for="password">Password:</Label>
                        <Input type="password" name="password" id="password" value={this.state.password} onChange={this.handleChange} placeholder="Password" />
                    </FormGroup>
                    <Button type="submit" color="primary" disabled={!this.validateForm() && !this.state.isSubmitting} >Sign In</Button>
                </Form>
            </div>
        )
    }
}
