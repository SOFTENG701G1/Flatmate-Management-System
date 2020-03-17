import React from 'react';
import { Redirect, Link } from 'react-router-dom';
import APIRequest from '../Util/APIRequest';
import '../App.css';
import './Members.css';
import AddMember from './AddMember';
import NewFlat from './NewFlat'

export default class MembersPage extends React.Component {
    constructor(props) {
        super(props);


        this.state = {
            items: [],
            isLoaded: false,
        }
        this.getMember();
        this.setFlatState = this.setFlatState.bind(this);
    }
    /* Function used to fetch JSON data from the API */
    async getMember() {
        const memberResult = await APIRequest.getFlatMembers()
        const json = await memberResult.json();
        this.setState({
            isLoaded: true,
            items: json
        })

    }

    setFlatState(json) {
        this.setState({
            items: json
        })
    }

    render() {
        var { isLoaded, items } = this.state;

        if (!isLoaded) {
            return (<div>Loading...</div>);
        }
        else {
            if (items.flatMembers.length === 0) {
                return (
                    <NewFlat setFlatState={this.setFlatState} />
                )
            }
            else {
                return (
                    <div className="MembersPage">
                        <div>
                            <div className='section-header'>
                                Members page
                </div>
                            <AddMember />
                            <h4 className='currentMember'>Current Members</h4>
                        </div>
                        <td className="MembersCells">
                            <table class="MembersModule">
                                <td>
                                    <div className="user_profile"> </div>
                                    <p> Name: {items.flatMembers[2].firstName} {items.flatMembers[2].lastName} </p>
                                    <p> Email: {items.flatMembers[2].email} </p>
                                    <p> Username: {items.flatMembers[2].userName}</p>
                                </td>
                            </table>
                        </td>
                        <td className="MembersCells">
                            <table class="MembersModule">
                                <td>
                                    <div className="user_profile"> </div>
                                    <p> Name: {items.flatMembers[1].firstName} {items.flatMembers[1].lastName} </p>
                                    <p> Email: {items.flatMembers[1].email} </p>
                                    <p> Username: {items.flatMembers[1].userName}</p>
                                </td>
                            </table>
                        </td>

                        <td className="MembersCells">
                            <table class="MembersModule">
                                <td>
                                    <div className="user_profile"> </div>
                                    <p> Name: {items.flatMembers[0].firstName} {items.flatMembers[0].lastName} </p>
                                    <p> Email: {items.flatMembers[0].email} </p>
                                    <p> Username: {items.flatMembers[0].userName}</p>
                                </td>
                            </table>
                        </td>


                    </div>);
            }
        }
    }
}
