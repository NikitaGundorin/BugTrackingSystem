import React, { Component } from 'react';
import { Button, Form, FormGroup, Label, Input, ButtonGroup, FormText, Spinner, Modal, ModalBody } from 'reactstrap';
import "./style.css";
import { bugService } from '../services/BugService';

export class UpdateBug extends Component {
    static displayName = UpdateBug.name;

    constructor(props) {
        super(props);
        this.state = {
            id: this.props.id,
            shortDescription: this.props.shortDescription,
            fullDescription: this.props.fullDescription,
            priority: this.props.priority,
            importance: this.props.importance,
            importanceId: null,
            priorityId: null,
            params: null,
            isLoad: false,
            isSubmitting: false,
            modal: false
        };

        this.handleChange.bind(this);
        this.toggle = this.toggle.bind(this);
    }
    
    async toggle() {
        this.setState(prevState => ({
            modal: !prevState.modal
        }));

        if (this.state.modal == false) {
            await bugService.getParams().then(params => this.setState({ params, isLoad: true }));
            console.log(this.state.params);
            let importanceId = this.state.params[0].find(i => i.name === this.state.importance).id;
            let priorityId = this.state.params[1].find(p => p.name === this.state.priority).id;
            this.setState({importanceId: importanceId, priorityId: priorityId});
        }
    }

    validateForm() {
        return this.state.shortDescription.length > 0 && this.state.shortDescription.length < 35 && this.state.fullDescription.length > 0;
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
        bugService.updateBug(this.state.id, this.state.shortDescription, this.state.fullDescription, this.state.priorityId, this.state.importanceId)
            .then(
                bug => {
                    this.toggle();
                    this.props.handler();
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
        const externalCloseBtn = <button className="close" style={{ position: 'absolute', top: '15px', right: '15px' }} onClick={this.toggle}>&times;</button>;

        return (
            <div style={{ display: "inline-block", marginLeft: "1rem" }}>
                <Button size="sm" outline color="primary" onClick={() => this.toggle()} style={{ marginBottom: "0.5rem" }}>Edit</Button>
                <Modal isOpen={this.state.modal} toggle={this.toggle} external={externalCloseBtn}>
                    <ModalBody>
                        {
                            this.state.isLoad ?
                                <Form className="updateBugForm" onSubmit={this.handleSubmit} style={{ maxWidth: "700px" }}>
                                    <h2>Edit Bug</h2>
                                    <FormGroup>
                                        <Label for="shortDescription">Short Description:</Label>
                                        <Input type="text" name="shortDescription" id="shortDescription" value={this.state.shortDescription} onChange={this.handleChange} placeholder="Short Description" />
                                        <FormText color="muted">
                                            Maximum 35 characters
                                    </FormText>
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
                                    <Button type="submit" color="primary" disabled={!this.validateForm() && !this.state.isSubmitting}>Save</Button>
                                </Form>
                                :
                                <div style={{ display: "flex", justifyContent: "center" }} >
                                    <Spinner color="primary" />
                                </div>
                        }
                    </ModalBody>
                </Modal>
            </div>
        )
    }


}