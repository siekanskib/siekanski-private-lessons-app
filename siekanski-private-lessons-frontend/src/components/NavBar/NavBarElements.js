import styled from 'styled-components'
import { NavLink as Link } from 'react-router-dom'
import {FaBars} from 'react-icons/fa'

export const Nav = styled.nav`
 background: #000;
 height: 80px;
 display: flex;
 justify-content: space-between;
 padding: 0.5rem calc((100vw - 1000px)) / 2);
 z-index: 10;
`

export const NavLink = styled(Link)`
color: #fff;
display: flex;
align-items: center;
text-decoration: none;
padding: 0 1rem;
height: 100%;
cursor: pointer;

&.active {
  color: #15cdfc;
}
`

export const Bars = styled(FaBars)`
display: none;
color: #fff;

@media screen and (max-width: 768px) {
  display: block;
  position: absolute;
  top: 0;
  right: 0;
  transform: translate(-100%, 75%);
  font-size: 1.8rem;
  cursor: pointer;
}
`

export const NavMenu = styled.div`
  display: flex;
  align-items: center;

  /* Second Nav */
  /* margin-right: 24px; */

  /* Third Nav */
  /* width: 100vw;
  white-space: nowrap; */

  @media screen and (max-width: 768px) {
    display: ${({ isOpen }) => (isOpen ? 'flex' : 'none')};
    flex-direction: column;
    width: 100%;
    position: absolute;
    top: 80px; 
    left: 0;
    background: #000; // Kolor tła menu
    height: calc(100vh - 80px); 
    z-index: 1; 
  }
`

export const NavBtnLink = styled(Link)`
border-radius: 4px;
background: #256cel;
padding: 10px 22px;
color: #fff;
border: none;
outline: none;
cursor: pointer;
transtion: all 0.2s ease-in-out;
text-decoration: none;

&:hover {
  transtion: all 0.2s ease-in-out;
  background: #fff;
  color:#010606;
}
`
