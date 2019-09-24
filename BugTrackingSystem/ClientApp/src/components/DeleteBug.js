import React, { Component } from 'react';
import { Button, Form, Modal, ModalBody } from 'reactstrap';
import "./style.css";
import { bugService } from '../services/BugService';
import { history } from '../App';

export class DeleteBug extends Component {
    static displayName = DeleteBug.name;

    constructor(props) {
        super(props);
        this.state = {
            id: this.props.id,
            isSubmitting: false,
            modal: false
        };

        this.toggle = this.toggle.bind(this);
    }

    async toggle() {
        this.setState(prevState => ({
            modal: !prevState.modal
        }));
    }

    handleSubmit = event => {
        event.preventDefault();
        this.setState({ isSubmitting: true });
        bugService.deleteBug(this.state.id)
            .then(
                bug => {
                    this.toggle();
                    history.push('/');
                },
                error => {
                    this.setState({ isSubmitting: false });
                    alert(error);
                }
            );
        this.setState({ isSubmitting: false });
    }

    render() {
        const externalCloseBtn = <button className="close" style={{ position: 'absolute', top: '15px', right: '15px' }} onClick={this.toggle}>&times;</button>;

        return (
            <div style={{ display: "inline-block", marginLeft: "1rem" }}>
                <Button size="sm" outline color="danger" onClick={() => this.toggle()} style={{ marginBottom: "0.5rem" }}>Delete</Button>
                <Modal isOpen={this.state.modal} toggle={this.toggle} external={externalCloseBtn}>
                    <ModalBody>
                        <Form className="updateBugForm" onSubmit={this.handleSubmit} style={{ maxWidth: "700px" }}>
                            <h2>Delete Bug #{this.state.id}?</h2>
                            <p>Are you sure you want to delete this bug?</p>
                            <p>This action cannot be undone.</p>
                            <Button type="submit" color="danger" className="formButton" disabled={this.state.isSubmitting}>Delete</Button>
                            <Button color="secondary" onClick={this.toggle}>Cancel</Button>
                        </Form>
                    </ModalBody>
                </Modal>
            </div>
        )
    }
}