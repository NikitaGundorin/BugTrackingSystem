import React, { Component } from 'react';
import { Button, Form, FormGroup, Label, Input, ButtonGroup } from 'reactstrap';
import "./style.css";
import { bugService } from '../services/BugService';

export class AddBug extends Component {
    static displayName = AddBug.name;

    constructor(props) {
        super(props);
        this.state = {
            shortDescription: "",
            fullDescription: "",
            priorityId: 1,
            importanceId: 1,
            params: null,
            isSubmitting: false
        };

        this.handleChange.bind(this);
    }

    componentDidMount() {
        bugService.getParams().then(params => this.setState({ params }));
    }

    validateForm() {
        return this.state.shortDescription.length > 0 && this.state.fullDescription.length > 0;
    }

    onRadioBtnClick(param, id) {
        param == 0 ? this.setState({ importanceId: id }) : this.setState({ priorityId: id });
    }

    handleChange = event => {
        this.setState({
            [event.target.id]: event.target.value
        });
    }

    handleSubmit = event => {
        event.preventDefault();
        this.setState({ isSubmitting: true });
        bugService.addBug(this.state.shortDescription, this.state.fullDescription, this.state.priorityId, this.state.importanceId)
            .then(
                bug => {
                    const { from } = this.props.location.state || { from: { pathname: "/" } };
                    this.props.history.push(from);
                },
                error => {
                    this.setState({ isSubmitting: false });
                    alert(error);
                }
            );
        this.setState({ isSubmitting: false });
    }

    render() {
        const importance = this.state.params ? this.state.params[0].map((importance) => <Button color="primary" onClick={() => this.onRadioBtnClick(0, importance.id)} active={this.state.importanceId === importance.id}>{importance.name}</Button>) : null;
        const priority = this.state.params ? this.state.params[1].map((priority) => <Button color="primary" onClick={() => this.onRadioBtnClick(1, priority.id)} active={this.state.priorityId === priority.id}>{priority.name}</Button>) : null;

        return (
            <div>
                <Form onSubmit={this.handleSubmit} style={{ maxWidth: "700px" }}>
                    <h2>Add Bug</h2>
                    <FormGroup>
                        <Label for="shortDescription">Short Description:</Label>
                        <Input type="text" name="shortDescription" id="shortDescription" value={this.state.shortDescription} onChange={this.handleChange} placeholder="Short Description" />
                    </FormGroup>
                    <FormGroup>
                        <Label for="fullDescription">Full Description:</Label>
                        <Input type="textarea" name="fullDescription" id="fullDescription" value={this.state.fullDescription} onChange={this.handleChange} placeholder="Full Description" />
                    </FormGroup>
                    <FormGroup>
                        <Label for="importance">Importance:</Label><br />
                        <ButtonGroup id="importance">
                            {importance}
                        </ButtonGroup>
                    </FormGroup>
                    <FormGroup>
                        <Label for="priority">Priority:</Label><br />
                        <ButtonGroup id="priority">
                            {priority}
                        </ButtonGroup>
                    </FormGroup>
                    <Button type="submit" color="primary" disabled={!this.validateForm() && !this.state.isSubmitting} >Add</Button>
                </Form>
            </div>
        )
    }


}