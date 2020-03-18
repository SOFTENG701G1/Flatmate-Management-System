import React from 'react';
import { Redirect, Link } from 'react-router-dom';
import APIRequest from '../Util/APIRequest';
import '../App.css';
import './Members.css';
import AddMember from './AddMember';
import MemberTile from './MemberTile';
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
        else
            return (
                <div className="MembersPage">
                    <div>
                        <div className='section-header'>Members page</div>
                        <AddMember />
                        <h4 className='currentMember'>Current Members</h4>
                        {this.state.items.map(function (flatMember, i) {
                            return <MemberTile key={i} flatMember={flatMember} />
                        })}
                    </div>
                </div>);
    }
}
