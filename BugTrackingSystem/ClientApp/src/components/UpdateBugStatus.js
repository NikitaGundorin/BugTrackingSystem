import React, { Component } from 'react';
import { Button, Form, FormGroup, Label, Input, Modal, ModalBody } from 'reactstrap';
import { bugService } from '../services/BugService';

export class UpdateBugStatus extends Component {
    static displayName = UpdateBugStatus.name;

    constructor(props) {
        super(props);
        this.state = {
            bugId: this.props.bugId,
            comment: "",
            newStatusId: this.props.newStatusId,
            isSubmitting: false,
            modal: false
        };

        this.handleChange.bind(this);
        this.toggle = this.toggle.bind(this);
    }

    validateForm() {
        return this.state.comment.length > 0;
    }

    toggle() {
        this.setState(prevState => ({
            modal: !prevState.modal
        }));
    }

    handleChange = event => {
        this.setState({
            [event.target.id]: event.target.value
        });
    }

    handleSubmit = event => {
        event.preventDefault();
        this.setState({ isSubmitting: true });
        bugService.updateBugStatus(this.state.bugId, this.state.newStatusId, this.state.comment)
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
    }

    render() {
        const externalCloseBtn = <button className="close" style={{ position: 'absolute', top: '15px', right: '15px' }} onClick={this.toggle}>&times;</button>;
        return (
            <div>
                <Button size="sm" outline color={this.props.color} onClick={this.toggle} style={{lineHeight: "1rem", margin:"0 0.2rem"}}>{this.props.action}</Button>
                <Modal isOpen={this.state.modal} toggle={this.toggle} external={externalCloseBtn}>
                    <ModalBody>
                        <Form onSubmit={this.handleSubmit} className="updateBugStatusForm">
                            <h2>{this.props.action} Bug</h2>
                            <FormGroup>
                                <Label for="comment">Comment:</Label>
                                <Input type="textarea" name="comment" id="comment" value={this.state.shortDescription} onChange={this.handleChange} placeholder="Comment" />
                            </FormGroup>
                            <Button className="formButton" type="submit" color="primary" disabled={!this.validateForm() && !this.state.isSubmitting} >Submit</Button>
                            <Button color="secondary" onClick={this.toggle}>Cancel</Button>
                        </Form>
                    </ModalBody>
                </Modal>
            </div>
        )
    }
}