// Add BeethovenMain.initializeHacks(); to PlayerManager
﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Dissonance.Integrations.UNet_HLAPI;
using UnityEngine;

// Token: 0x0200047E RID: 1150
public class BeethovenMain : MonoBehaviour
{
	// Token: 0x06001A3D RID: 6717
	private void Awake()
	{
		this.someIndex = 0;
		BeethovenMain.Instance = this;
		this.currentHacksIndex = 0;
		this.currentMaxLines = 0;
		this.disabledDoors = new List<Door>();
		this.targetPositions = new List<Vector3>();
		this.tools = new List<BeethovenOption>();
		this.tools.Add(new BeethovenOption("maximumSpeed", BeethovenOptionType.TOGGLE, "Speedhack <color=#aaaaaa>(17% additional running-speed)</color>", "Movement"));
		this.tools.Add(new BeethovenOption("flight", BeethovenOptionType.TOGGLE, "Freecam <color=#aaaaaa>(allows you to look around the map)</color>", "Movement"));
		this.tools.Add(new BeethovenOption("teleportMode", BeethovenOptionType.TOGGLE, "Teleportation Mode <color=#aaaaaa>(perform short range teleports every second)</color>", "Movement"));
		this.tools.Add(new BeethovenOption("trueFlight", BeethovenOptionType.TOGGLE, "Fly Hack <color=#aaaaaa>(beware of anti-cheat)</color>", "Movement"));
		this.tools.Add(new BeethovenOption("doorMaster", BeethovenOptionType.TOGGLE, "Anti-Doors <color=#aaaaaa>(closes doors and disables them for your client)</color>", "Movement"));
		this.tools.Add(new BeethovenOption("followTarget", BeethovenOptionType.TOGGLE, "Follow Target <color=#77FF77aa>[F]</color> <color=#aaaaaa>(requires a target, follows target while you freecam)</color>", "Movement"));
		this.tools.Add(new BeethovenOption("playerWallhack", BeethovenOptionType.TOGGLE, "Player Wallhack", "Visual"));
		this.tools.Add(new BeethovenOption("itemWallhack", BeethovenOptionType.TOGGLE, "Item Wallhack", "Visual"));
		this.tools.Add(new BeethovenOption("locationsWallhack", BeethovenOptionType.TOGGLE, "Location ESP <color=#aaaaaa>(see Lifts and 914's location)</color>", "Visual"));
		this.tools.Add(new BeethovenOption("selectTarget", BeethovenOptionType.METHOD, "Select Target <color=#77FF77aa>[C]</color> <color=#aaaaaa>(Aim at him and use this to select the target)</color>", "Combat"));
		this.tools.Add(new BeethovenOption("killTarget", BeethovenOptionType.TOGGLE, "Kill Target <color=#77FF77aa>[V]</color> <color=#aaaaaa>(requires a target & gun in hand)</color>", "Combat"));
		this.tools.Add(new BeethovenOption("killEveryone", BeethovenOptionType.TOGGLE, "Kill Everyone <color=#aaaaaa>(requires a gun in hand)</color>", "Combat"));
		this.tools.Add(new BeethovenOption("noRecoil", BeethovenOptionType.TOGGLE, "No Recoil <color=#aaaaaa>(Removes recoil and bullet spread)</color>", "Combat"));
		this.tools.Add(new BeethovenOption("scpMode", BeethovenOptionType.TOGGLE, "SCP MODE <color=#aaaaaa>(auto-aimbot for SCP's, just spam leftclick)</color>", "Combat"));
		this.tools.Add(new BeethovenOption("hidSpam", BeethovenOptionType.TOGGLE, "Micro-HID Earrape <color=#aaaaaa>(may no longer work)</color>", "Trolling"));
		this.tools.Add(new BeethovenOption("keepHandcuffing", BeethovenOptionType.TOGGLE, "Item Dropper <color=#aaaaaa>(may no longer work, requires handcuffs in hand)</color>", "Trolling"));
		this.tools.Add(new BeethovenOption("quickItemPickup", BeethovenOptionType.TOGGLE, "Fast Item Pickup <color=#aaaaaa>(pick up items instantly)</color>", "Extras"));
		this.tools.Add(new BeethovenOption("autoPickup", BeethovenOptionType.TOGGLE, "Item Magnet <color=#aaaaaa>(Press E to pickup nearby items)</color>", "Extras"));
		this.tools.Add(new BeethovenOption("antiTesla", BeethovenOptionType.TOGGLE, "Electricman <color=#aaaaaa>(Tesla-Gate Immunity)</color>", "Extras"));
		this.initializeCategories();
		base.InvokeRepeating("updateVariables", 0.12f, 0.5f);
		base.InvokeRepeating("slowHackRepeats", 0f, 0.05f);
		base.InvokeRepeating("checkTesla", 0.35f, 4f);
		base.InvokeRepeating("updateLocations1", 0.59f, 4f);
		base.InvokeRepeating("updateLocations2", 0.19f, 1f);
		this.playerWallhack = true;
		this.noRecoil = true;
		this.maximumSpeed = true;
		this.quickItemPickup = false;
		this.locationsWallhack = false;
		this.antiTesla = true;
		this.recoilProperties = new RecoilProperties();
		this.recoilProperties.backSpeed = 0f;
		this.recoilProperties.upSize = 0f;
		this.recoilProperties.fovKick = 0f;
		this.recoilProperties.lerpSpeed = 0f;
		this.recoilProperties.shockSize = 0f;
	}

	// Token: 0x06001A3E RID: 6718 RVA: 0x0009CA64 File Offset: 0x0009AC64
	private void Update()
	{
		if (this.myCCM == null)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.F))
		{
			this.followTarget = !this.followTarget;
		}
		if (Input.GetKeyDown(KeyCode.V))
		{
			this.killTarget = !this.killTarget;
		}
		if (Input.GetKeyDown(KeyCode.C))
		{
			this.selectTarget();
		}
		if (this.stalk173 && this.player173 && !this.flight)
		{
			if (this.playerMovement.enabled)
			{
				this.playerMovement.enabled = false;
			}
			this.realPosition = base.transform.position;
			Vector3 eulerAngles = PlayerManager.localPlayer.transform.eulerAngles;
			Vector3 localEulerAngles = Camera.main.transform.localEulerAngles;
			Camera.main.transform.LookAt(this.player173);
			PlayerManager.localPlayer.transform.eulerAngles = new Vector3(0f, PlayerManager.localPlayer.transform.eulerAngles.y + Camera.main.transform.localEulerAngles.y, 0f);
			Camera.main.transform.localEulerAngles = new Vector3(Camera.main.transform.localEulerAngles.x, 0f, 0f);
			this.playerMovement.GetType().GetMethod("TransmitData", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(this.playerMovement, new object[0]);
			PlayerManager.localPlayer.transform.eulerAngles = eulerAngles;
			Camera.main.transform.localEulerAngles = localEulerAngles;
		}
		if (this.flight || (this.followTarget && this.target))
		{
			Vector3 a = Vector3.zero;
			if (Input.GetKey(KeyCode.W))
			{
				a += Camera.main.transform.forward;
			}
			if (Input.GetKey(KeyCode.S))
			{
				a += -Camera.main.transform.forward;
			}
			if (Input.GetKey(KeyCode.D))
			{
				a += Camera.main.transform.right;
			}
			if (Input.GetKey(KeyCode.A))
			{
				a += -Camera.main.transform.right;
			}
			float d = 10f;
			if (Input.GetKey(KeyCode.LeftShift))
			{
				d = 40f;
			}
			PlayerManager.localPlayer.transform.position += a.normalized * d * Time.deltaTime;
			if (Input.GetKey(KeyCode.Space))
			{
				PlayerManager.localPlayer.transform.position += Vector3.up * Time.deltaTime * 10f;
				if (Input.GetKey(KeyCode.LeftShift))
				{
					PlayerManager.localPlayer.transform.position += Vector3.up * Time.deltaTime * 200f;
				}
			}
			if (Input.GetKey(KeyCode.LeftControl))
			{
				PlayerManager.localPlayer.transform.position -= Vector3.up * Time.deltaTime * 4f;
				if (Input.GetKey(KeyCode.LeftShift))
				{
					PlayerManager.localPlayer.transform.position -= Vector3.up * Time.deltaTime * 200f;
				}
			}
			if (this.playerMovement.enabled)
			{
				this.playerMovement.enabled = false;
				this.playerMovement.GetType().GetMethod("TransmitData", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(this.playerMovement, new object[0]);
				this.realPosition = PlayerManager.localPlayer.transform.position;
				this.targetPositions = new List<Vector3>();
			}
			if (this.target && this.followTarget)
			{
				Vector3 position = PlayerManager.localPlayer.transform.position;
				Vector3 vector = this.target.transform.position;
				if (this.targetPositions.Count > 0)
				{
					vector = this.targetPositions[0];
				}
				if (Vector3.Distance(this.realPosition, this.target.transform.position) > 0.2f)
				{
					this.realPosition += (vector - this.realPosition) * Time.deltaTime * this.myCCM.klasy[this.myCCM.curClass].runSpeed * 1.2f;
					if (this.targetPositions.Count > 0 && Vector3.Distance(this.realPosition, vector) < 0.2f)
					{
						this.targetPositions.RemoveAt(0);
					}
				}
				else
				{
					this.targetPositions = new List<Vector3>();
				}
				PlayerManager.localPlayer.transform.position = this.realPosition;
				this.playerMovement.GetType().GetMethod("TransmitData", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(this.playerMovement, new object[0]);
				PlayerManager.localPlayer.transform.position = position;
			}
		}
		if (this.trueFlight && !this.flight)
		{
			Vector3 vector2 = Vector3.zero;
			if (Input.GetKey(KeyCode.W))
			{
				vector2 = Camera.main.transform.forward;
			}
			if (Input.GetKey(KeyCode.S))
			{
				vector2 = -Camera.main.transform.forward;
			}
			if (Input.GetKey(KeyCode.D))
			{
				vector2 = Camera.main.transform.right;
			}
			if (Input.GetKey(KeyCode.A))
			{
				vector2 = -Camera.main.transform.right;
			}
			float d2 = this.myCCM.klasy[this.myCCM.curClass].runSpeed * 1.25f;
			PlayerManager.localPlayer.transform.position += vector2.normalized * d2 * Time.deltaTime;
			if (Input.GetKey(KeyCode.Space))
			{
				PlayerManager.localPlayer.transform.position += Vector3.up * Time.deltaTime * 4f;
				if (Input.GetKey(KeyCode.LeftShift))
				{
					PlayerManager.localPlayer.transform.position += Vector3.up * Time.deltaTime * 200f;
				}
			}
			if (Input.GetKey(KeyCode.LeftControl))
			{
				PlayerManager.localPlayer.transform.position -= Vector3.up * Time.deltaTime * 4f;
				if (Input.GetKey(KeyCode.LeftShift))
				{
					PlayerManager.localPlayer.transform.position -= Vector3.up * Time.deltaTime * 200f;
				}
			}
		}
		if (this.teleportMode && !this.flight && (!this.followTarget || !this.target) && !this.trueFlight)
		{
			this.lastTeleport -= Time.deltaTime;
			Vector3 vector3 = Vector3.zero;
			if (Input.GetKey(KeyCode.W))
			{
				vector3 = PlayerManager.localPlayer.transform.forward;
			}
			else if (Input.GetKey(KeyCode.S))
			{
				vector3 = -PlayerManager.localPlayer.transform.forward;
			}
			else if (Input.GetKey(KeyCode.D))
			{
				vector3 = PlayerManager.localPlayer.transform.right;
			}
			else if (Input.GetKey(KeyCode.A))
			{
				vector3 = -PlayerManager.localPlayer.transform.right;
			}
			if (vector3 != Vector3.zero && this.lastTeleport <= 0f)
			{
				this.cc.enabled = true;
				this.cc.Move(vector3 * this.myCCM.klasy[this.myCCM.curClass].runSpeed * 1.29f);
				this.cc.Move(Vector3.down * 10f);
				this.lastTeleport = 1f;
			}
			this.cc.enabled = false;
		}
		if (this.quickItemPickup)
		{
			this.mySearching.GetType().GetField("timeToPickUp", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(this.mySearching, 0f);
		}
		if (this.autoPickup && this.myCCM.IsHuman() && Input.GetKey(NewInput.GetKey("Interact")))
		{
			foreach (Pickup pickup in UnityEngine.Object.FindObjectsOfType<Pickup>())
			{
				if (Vector3.Distance(pickup.transform.position, PlayerManager.localPlayer.transform.position) < 4f)
				{
					this.mySearching.GetType().GetField("pickup", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(this.mySearching, pickup.gameObject);
					this.mySearching.GetType().GetMethod("ContinuePickup", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(this.mySearching, new object[0]);
					this.mySearching.GetType().GetField("timeToPickUp", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(this.mySearching, 0f);
				}
			}
		}
		if (this.hidSpam)
		{
			this.myHID.Invoke("CallCmdUse", 0f);
			this.myHID.StartCoroutine("PlayAnimation");
		}
		if (this.killTarget && this.target)
		{
			CharacterClassManager component = this.target.GetComponent<CharacterClassManager>();
			if (component && component.curClass != 2)
			{
				Vector3 localEulerAngles2 = Camera.main.transform.localEulerAngles;
				Vector3 localPosition = Camera.main.transform.localPosition;
				Camera.main.transform.position = this.target.transform.position + Vector3.up * 1.7f;
				Camera.main.transform.LookAt(this.target.transform.position);
				RaycastHit raycastHit;
				Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out raycastHit, 10000f, this.myWeapons.raycastMask);
				if (raycastHit.collider != null)
				{
					HitboxIdentity hitboxIdentity = raycastHit.collider.GetComponent<HitboxIdentity>();
					if (hitboxIdentity == null)
					{
						hitboxIdentity = raycastHit.collider.gameObject.AddComponent<HitboxIdentity>();
					}
					hitboxIdentity.id = "HEAD";
					Camera.main.transform.position = localPosition;
					Camera.main.transform.LookAt(raycastHit.collider.transform.position);
				}
				this.myWeapons.GetType().GetMethod("Shoot", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(this.myWeapons, new object[0]);
				Camera.main.transform.localPosition = localPosition;
				Camera.main.transform.localEulerAngles = localEulerAngles2;
			}
		}
		if (this.killEveryone)
		{
			if (this.someIndex > PlayerManager.singleton.players.Length - 1 || this.someIndex < 0)
			{
				this.someIndex = 0;
			}
			bool flag = false;
			while (!flag || this.someIndex < PlayerManager.singleton.players.Length)
			{
				GameObject gameObject = PlayerManager.singleton.players[this.someIndex];
				CharacterClassManager component2 = gameObject.GetComponent<CharacterClassManager>();
				if (component2 && component2.curClass != 2 && this.checkValidTarget(gameObject))
				{
					flag = true;
					Vector3 localEulerAngles3 = Camera.main.transform.localEulerAngles;
					Vector3 localPosition2 = Camera.main.transform.localPosition;
					Camera.main.transform.position = gameObject.transform.position + Vector3.up * 1.7f;
					Camera.main.transform.LookAt(gameObject.transform.position);
					RaycastHit raycastHit2;
					Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.transform.forward), out raycastHit2, 10000f, this.myWeapons.raycastMask);
					if (raycastHit2.collider != null)
					{
						HitboxIdentity hitboxIdentity2 = raycastHit2.collider.GetComponent<HitboxIdentity>();
						if (hitboxIdentity2 == null)
						{
							hitboxIdentity2 = raycastHit2.collider.gameObject.AddComponent<HitboxIdentity>();
						}
						hitboxIdentity2.id = "HEAD";
						Camera.main.transform.position = localPosition2;
						Camera.main.transform.LookAt(raycastHit2.collider.transform.position);
					}
					this.myWeapons.GetType().GetMethod("Shoot", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(this.myWeapons, new object[0]);
					Camera.main.transform.localPosition = localPosition2;
					Camera.main.transform.localEulerAngles = localEulerAngles3;
				}
				this.someIndex++;
			}
		}
		if (this.scpMode)
		{
			if (Input.GetKey(NewInput.GetKey("Interact")))
			{
				Scp049PlayerScript component3 = this.myCCM.GetComponent<Scp049PlayerScript>();
				if (component3.iAm049)
				{
					foreach (Ragdoll ragdoll in UnityEngine.Object.FindObjectsOfType<Ragdoll>())
					{
						foreach (GameObject gameObject2 in PlayerManager.singleton.players)
						{
							if (gameObject2.GetComponent<HlapiPlayer>().PlayerId == ragdoll.owner.ownerHLAPI_id)
							{
								component3.recallProgress = 10f;
								component3.GetType().GetField("recallingObject", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(component3, gameObject2);
								component3.GetType().GetField("recallingRagdoll", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(component3, ragdoll);
								component3.GetType().GetMethod("Recalling", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(component3, new object[0]);
							}
						}
					}
				}
			}
			if (Input.GetMouseButtonDown(0))
			{
				Scp173PlayerScript component4 = this.myCCM.GetComponent<Scp173PlayerScript>();
				Scp049PlayerScript component5 = this.myCCM.GetComponent<Scp049PlayerScript>();
				Scp049_2PlayerScript component6 = this.myCCM.GetComponent<Scp049_2PlayerScript>();
				Scp106PlayerScript component7 = this.myCCM.GetComponent<Scp106PlayerScript>();
				Scp096PlayerScript component8 = this.myCCM.GetComponent<Scp096PlayerScript>();
				Scp939PlayerScript component9 = this.myCCM.GetComponent<Scp939PlayerScript>();
				if (component9 != null)
				{
					foreach (GameObject gameObject3 in PlayerManager.singleton.players)
					{
						if (this.checkValidTarget(gameObject3) && Vector3.Distance(PlayerManager.localPlayer.transform.position, gameObject3.transform.position) < component9.attackDistance)
						{
							Vector3 position2 = Camera.main.transform.position;
							Vector3 eulerAngles2 = Camera.main.transform.eulerAngles;
							Camera.main.transform.position = gameObject3.transform.position + Vector3.up * 1.5f;
							Camera.main.transform.LookAt(gameObject3.transform.position);
							component9.GetType().GetMethod("Shoot", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(component9, new object[0]);
							Camera.main.transform.position = position2;
							Camera.main.transform.eulerAngles = eulerAngles2;
							break;
						}
					}
				}
				if (component4.iAm173)
				{
					foreach (GameObject gameObject4 in PlayerManager.singleton.players)
					{
						if (this.checkValidTarget(gameObject4) && Vector3.Distance(PlayerManager.localPlayer.transform.position, gameObject4.transform.position) < 5f)
						{
							component4.GetType().GetMethod("HurtPlayer", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(component4, new object[]
							{
								gameObject4,
								"123"
							});
						}
					}
				}
				if (component5.iAm049)
				{
					foreach (GameObject gameObject5 in PlayerManager.singleton.players)
					{
						if (this.checkValidTarget(gameObject5) && Vector3.Distance(PlayerManager.localPlayer.transform.position, gameObject5.transform.position) < component5.distance * 1.3f)
						{
							component5.GetType().GetMethod("InfectPlayer", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(component5, new object[]
							{
								gameObject5,
								"123"
							});
						}
					}
				}
				if (component6.iAm049_2)
				{
					foreach (GameObject gameObject6 in PlayerManager.singleton.players)
					{
						if (this.checkValidTarget(gameObject6))
						{
							int curClass = gameObject6.GetComponent<CharacterClassManager>().curClass;
							gameObject6.GetComponent<CharacterClassManager>().curClass = 1;
							if (this.checkValidTarget(gameObject6) && Vector3.Distance(PlayerManager.localPlayer.transform.position, gameObject6.transform.position) < component6.distance * 1.45f)
							{
								gameObject6.GetComponent<Scp049_2PlayerScript>().sameClass = false;
								Vector3 localEulerAngles4 = Camera.main.transform.localEulerAngles;
								Camera.main.transform.LookAt(gameObject6.transform.position + Vector3.up);
								component6.GetType().GetMethod("Attack", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(component6, new object[0]);
								Camera.main.transform.localEulerAngles = localEulerAngles4;
							}
							gameObject6.GetComponent<CharacterClassManager>().curClass = curClass;
						}
					}
				}
				if (component7.iAm106)
				{
					component7.StopCoroutine("ContainAnimation");
					foreach (GameObject gameObject7 in PlayerManager.singleton.players)
					{
						if (this.checkValidTarget(gameObject7))
						{
							int curClass2 = gameObject7.GetComponent<CharacterClassManager>().curClass;
							gameObject7.GetComponent<CharacterClassManager>().curClass = 1;
							if (this.checkValidTarget(gameObject7) && Vector3.Distance(PlayerManager.localPlayer.transform.position, gameObject7.transform.position) < 3f)
							{
								Vector3 localEulerAngles5 = Camera.main.transform.localEulerAngles;
								Camera.main.transform.LookAt(gameObject7.transform.position + Vector3.up);
								MethodInfo method = component7.GetType().GetMethod("Shoot", BindingFlags.Instance | BindingFlags.NonPublic);
								method.Invoke(component7, new object[0]);
								method.Invoke(component7, new object[0]);
								method.Invoke(component7, new object[0]);
								Camera.main.transform.localEulerAngles = localEulerAngles5;
							}
							gameObject7.GetComponent<CharacterClassManager>().curClass = curClass2;
						}
					}
				}
				if (component8.iAm096)
				{
					foreach (GameObject gameObject8 in PlayerManager.singleton.players)
					{
						if (this.checkValidTarget(gameObject8))
						{
							int curClass3 = gameObject8.GetComponent<CharacterClassManager>().curClass;
							gameObject8.GetComponent<CharacterClassManager>().curClass = 1;
							if (Vector3.Distance(PlayerManager.localPlayer.transform.position, gameObject8.transform.position) < 3f)
							{
								Vector3 localEulerAngles6 = Camera.main.transform.localEulerAngles;
								Camera.main.transform.LookAt(gameObject8.transform.position + Vector3.up);
								component8.GetType().GetMethod("Shoot", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(component8, new object[0]);
								Camera.main.transform.localEulerAngles = localEulerAngles6;
							}
							gameObject8.GetComponent<CharacterClassManager>().curClass = curClass3;
						}
					}
				}
			}
		}
	}

	// Token: 0x06001A3F RID: 6719 RVA: 0x0009DF2C File Offset: 0x0009C12C
	private void checkTesla()
	{
		foreach (TeslaGate teslaGate in UnityEngine.Object.FindObjectsOfType<TeslaGate>())
		{
			teslaGate.sizeOfKiller = Vector3.zero;
			teslaGate.killers = new GameObject[0];
			teslaGate.sizeOfTrigger = -0.05f;
		}
	}

	// Token: 0x06001A40 RID: 6720 RVA: 0x0009DF74 File Offset: 0x0009C174
	private void handcuffNearby()
	{
		int num = 0;
		Handcuffs component = PlayerManager.localPlayer.GetComponent<Handcuffs>();
		Transform transform = Camera.main.transform;
		foreach (GameObject gameObject in PlayerManager.singleton.players)
		{
			if (this.checkValidTarget(gameObject) && Vector3.Distance(gameObject.transform.position, PlayerManager.localPlayer.transform.position) < 4f)
			{
				num++;
				CharacterClassManager component2 = gameObject.GetComponent<CharacterClassManager>();
				int curClass = component2.curClass;
				Vector3 eulerAngles = transform.eulerAngles;
				Vector3 position = transform.position;
				component2.curClass = 1;
				if (this.myCCM.curClass == 1 || this.myCCM.curClass == 8)
				{
					component2.curClass = 11;
				}
				transform.position = gameObject.transform.position + Vector3.up;
				transform.LookAt(gameObject.transform.position);
				component.GetType().GetMethod("CuffPlayer", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(component, new object[0]);
				transform.position = position;
				transform.eulerAngles = eulerAngles;
				component2.curClass = curClass;
			}
		}
	}

	// Token: 0x06001A41 RID: 6721 RVA: 0x0009E0B4 File Offset: 0x0009C2B4
	private void slowHackRepeats()
	{
		// GIving error
		// PlayerManager.localPlayer.transform.position - this.myLastPosition;
		if (this.maximumSpeed)
		{
			this.myFPC.m_RunSpeed = this.myCCM.klasy[this.myCCM.curClass].runSpeed * 1.17f;
		}
		else
		{
			this.myFPC.m_RunSpeed = this.myCCM.klasy[this.myCCM.curClass].runSpeed;
		}
		if (this.stalk173)
		{
			this.player173 = null;
			foreach (GameObject gameObject in PlayerManager.singleton.players)
			{
				if (this.checkValidTarget(gameObject) && gameObject.GetComponent<Scp173PlayerScript>().iAm173)
				{
					this.player173 = gameObject.transform;
					break;
				}
			}
		}
		if (this.keepHandcuffing)
		{
			this.handcuffNearby();
		}
		if (this.disabledDoors.Count > 0)
		{
			foreach (Door door in this.disabledDoors)
			{
				if (door != null)
				{
					door.gameObject.SetActive(true);
				}
			}
			this.disabledDoors = new List<Door>();
		}
		if (this.doorMaster)
		{
			foreach (Door door2 in UnityEngine.Object.FindObjectsOfType<Door>())
			{
				float num = Vector3.Distance(PlayerManager.localPlayer.transform.position, door2.transform.position);
				float num2 = 6f;
				if (this.teleportMode)
				{
					num2 += 15f;
				}
				if (num <= num2)
				{
					if (door2.isOpen)
					{
						this.pi.CallCmdOpenDoor(door2.gameObject);
					}
					door2.gameObject.SetActive(false);
					this.disabledDoors.Add(door2);
				}
			}
		}
		this.myCCM.GetComponent<FallDamage>().enabled = false;
		this.cc.enabled = (!this.flight && !this.teleportMode && (!this.followTarget || !this.target));
		PlayerManager.localPlayer.GetComponent<FallDamage>().enabled = false;
		if (!this.playerMovement.enabled && !this.flight && (!this.followTarget || !this.target) && !this.stalk173)
		{
			base.transform.position = this.realPosition;
			this.playerMovement.enabled = true;
		}
		if (this.noRecoil && this.myWeapons != null && this.myWeapons.weapons != null && this.myWeapons.weapons.Length > this.myWeapons.curWeapon && this.myWeapons.weapons[this.myWeapons.curWeapon] != null)
		{
			this.myWeapons.weapons[this.myWeapons.curWeapon].recoilAnimation = 0f;
			this.myWeapons.weapons[this.myWeapons.curWeapon].recoil = this.recoilProperties;
			this.myWeapons.weapons[this.myWeapons.curWeapon].unfocusedSpread = 0f;
		}
		this.myLastPosition = PlayerManager.localPlayer.transform.position;
	}

	// Token: 0x06001A42 RID: 6722 RVA: 0x0009E424 File Offset: 0x0009C624
	private void selectTarget()
	{
		float num = 15f;
		GameObject y = null;
		foreach (GameObject gameObject in PlayerManager.singleton.players)
		{
			Vector3 position = gameObject.transform.position;
			if (this.checkValidTarget(gameObject) && Vector3.Angle(Camera.main.transform.forward, position - Camera.main.transform.position) < num)
			{
				y = gameObject;
				num = Vector3.Angle(Camera.main.transform.forward, position - Camera.main.transform.position);
			}
		}
		if (this.target == y)
		{
			this.target = null;
			return;
		}
		this.target = y;
	}

	// Token: 0x06001A43 RID: 6723 RVA: 0x0001A4A9 File Offset: 0x000186A9
	private bool checkValidTarget(GameObject player)
	{
		return player != PlayerManager.localPlayer;
	}

	// Token: 0x06001A44 RID: 6724 RVA: 0x0001A4B6 File Offset: 0x000186B6
	private void updateLocations1()
	{
		this.lifts = UnityEngine.Object.FindObjectsOfType<Lift>();
	}

	// Token: 0x06001A45 RID: 6725 RVA: 0x0001A4C3 File Offset: 0x000186C3
	private void updateLocations2()
	{
		this.pdts = UnityEngine.Object.FindObjectsOfType<PocketDimensionTeleport>();
	}

	// Token: 0x06001A46 RID: 6726 RVA: 0x0009E4EC File Offset: 0x0009C6EC
	private void updateVariables()
	{
		if (PlayerManager.localPlayer == null)
		{
			return;
		}
		if (this.myCCM == null)
		{
			this.myCCM = PlayerManager.localPlayer.GetComponent<CharacterClassManager>();
		}
		if (this.myFPC == null)
		{
			this.myFPC = this.myCCM.GetComponent<FirstPersonController>();
		}
		if (this.mySR == null)
		{
			this.mySR = PlayerManager.localPlayer.GetComponent<ServerRoles>();
		}
		if (this.cam == null)
		{
			this.cam = Camera.main.transform;
		}
		if (this.mySearching == null)
		{
			this.mySearching = PlayerManager.localPlayer.GetComponent<Searching>();
		}
		if (this.myHID == null)
		{
			this.myHID = PlayerManager.localPlayer.GetComponent<MicroHID_GFX>();
		}
		if (this.myInventory == null)
		{
			this.myInventory = PlayerManager.localPlayer.GetComponent<Inventory>();
		}
		if (this.playerMovement == null)
		{
			this.playerMovement = PlayerManager.localPlayer.GetComponent<PlyMovementSync>();
		}
		if (this.cc == null)
		{
			this.cc = PlayerManager.localPlayer.GetComponent<CharacterController>();
		}
		if (this.pi == null)
		{
			this.pi = PlayerManager.localPlayer.GetComponent<PlayerInteract>();
		}
		if (this.ps == null)
		{
			this.ps = PlayerManager.localPlayer.GetComponent<PlayerStats>();
		}
		if (this.myWeapons == null)
		{
			this.myWeapons = PlayerManager.localPlayer.GetComponent<WeaponManager>();
		}
	}

	// Token: 0x06001A47 RID: 6727 RVA: 0x0009E670 File Offset: 0x0009C870
	private void LateUpdate()
	{
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			this.currentHacksIndex++;
			if (this.currentHacksIndex >= this.currentMaxLines)
			{
				this.currentHacksIndex = this.currentMaxLines - 1;
			}
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			this.currentHacksIndex--;
			if (this.currentHacksIndex < 0)
			{
				this.currentHacksIndex = 0;
			}
		}
		if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			this.influenceCurrent = true;
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			this.exitCategory = true;
		}
	}

	// Token: 0x06001A48 RID: 6728 RVA: 0x0009E710 File Offset: 0x0009C910
	private void OnGUI()
	{
		this.DefaultGUI();
		if (this.myCCM != null && this.playerWallhack)
		{
			foreach (GameObject gameObject in PlayerManager.singleton.players)
			{
				if (gameObject != PlayerManager.localPlayer && gameObject.GetComponent<CharacterClassManager>().curClass != 2 && Vector3.Angle(Camera.main.transform.forward, gameObject.transform.position - Camera.main.transform.position) < 70f)
				{
					Vector3 vector = Camera.main.WorldToScreenPoint(gameObject.transform.position);
					GUIStyle guistyle = new GUIStyle();
					guistyle.normal.textColor = this.myCCM.klasy[gameObject.GetComponent<CharacterClassManager>().curClass].classColor;
					string text = "";
					if (this.target == gameObject)
					{
						text = "<color=#ff2222ff>[Target]</color> ";
					}
					GUI.Label(new Rect(vector.x - 20f, (float)Screen.height - vector.y - 20f, vector.x + 40f, (float)Screen.height - vector.y + 50f), string.Concat(new object[]
					{
						text,
						(int)Vector3.Distance(Camera.main.transform.position, gameObject.transform.position),
						"m  HP: ",
						gameObject.GetComponent<PlayerStats>().health,
						"\n",
						gameObject.GetComponent<NicknameSync>().NetworkmyNick,
						" (",
						this.myCCM.klasy[gameObject.GetComponent<CharacterClassManager>().curClass].fullName,
						")"
					}), guistyle);
				}
			}
		}
		if (this.myInventory != null && this.itemWallhack)
		{
			foreach (Pickup pickup in UnityEngine.Object.FindObjectsOfType<Pickup>())
			{
				if (Vector3.Angle(Camera.main.transform.forward, pickup.transform.position - Camera.main.transform.position) < 70f)
				{
					Vector3 vector2 = Camera.main.WorldToScreenPoint(pickup.transform.position);
					GUIStyle guistyle2 = new GUIStyle();
					guistyle2.normal.textColor = new Color(1f, 1f, 0.7f, 0.5f);
					GUI.Label(new Rect(vector2.x - 20f, (float)Screen.height - vector2.y - 20f, vector2.x + 40f, (float)Screen.height - vector2.y + 50f), string.Concat(new object[]
					{
						this.myInventory.availableItems[pickup.info.itemId].label,
						" at ",
						(int)Vector3.Distance(Camera.main.transform.position, pickup.transform.position),
						"m"
					}), guistyle2);
				}
			}
		}
		if (this.myCCM != null && this.locationsWallhack)
		{
			this.displayLocation("SCP_914", GameObject.FindGameObjectWithTag("914_use").transform.position);
			Lift[] array2 = this.lifts;
			for (int i = 0; i < array2.Length; i++)
			{
				foreach (Lift.Elevator elevator in array2[i].elevators)
				{
					if (Mathf.Abs(PlayerManager.localPlayer.transform.position.y - elevator.door.transform.position.y) < 150f)
					{
						this.displayLocation("Lift", elevator.door.transform.position);
					}
				}
			}
			foreach (TeslaGate teslaGate in UnityEngine.Object.FindObjectsOfType<TeslaGate>())
			{
				this.displayLocation("Tesla", teslaGate.transform.position);
			}
			foreach (PocketDimensionTeleport pocketDimensionTeleport in this.pdts)
			{
				if (pocketDimensionTeleport.GetTeleportType() == PocketDimensionTeleport.PDTeleportType.Exit)
				{
					this.displayLocation("Exit", pocketDimensionTeleport.transform.position);
				}
			}
		}
		if (this.target && this.followTarget)
		{
			this.displayLocationCustom("<color=#22CCFFFF>[YOU]</color>", this.realPosition + Vector3.up * 1.5f);
		}
	}

	// Token: 0x06001A49 RID: 6729 RVA: 0x0009EBF8 File Offset: 0x0009CDF8
	public void displayLocation(string name, Vector3 thePosition)
	{
		if (Vector3.Angle(Camera.main.transform.forward, thePosition - Camera.main.transform.position) < 70f)
		{
			Vector3 vector = Camera.main.WorldToScreenPoint(thePosition);
			GUIStyle guistyle = new GUIStyle();
			guistyle.normal.textColor = new Color(1f, 0.65f, 1f, 0.6f);
			GUI.Label(new Rect(vector.x - 20f, (float)Screen.height - vector.y - 20f, vector.x + 40f, (float)Screen.height - vector.y + 50f), string.Concat(new object[]
			{
				name,
				" [",
				(int)Vector3.Distance(Camera.main.transform.position, thePosition),
				"m]"
			}), guistyle);
		}
	}

	// Token: 0x06001A4A RID: 6730 RVA: 0x0009ECF4 File Offset: 0x0009CEF4
	public void displayLocationCustom(string name, Vector3 thePosition)
	{
		if (Vector3.Angle(Camera.main.transform.forward, thePosition - Camera.main.transform.position) < 70f)
		{
			Vector3 vector = Camera.main.WorldToScreenPoint(thePosition);
			GUIStyle guistyle = new GUIStyle();
			guistyle.normal.textColor = new Color(1f, 0.65f, 1f, 0.6f);
			GUI.Label(new Rect(vector.x - 20f, (float)Screen.height - vector.y - 20f, vector.x + 40f, (float)Screen.height - vector.y + 50f), name, guistyle);
		}
	}

	// Token: 0x06001A4B RID: 6731 RVA: 0x0009EDB4 File Offset: 0x0009CFB4
	public void DefaultGUI()
	{
		GUI.Label(new Rect((float)(Screen.width - 100), 10f, 120f, 30f), "<color=#ff1111aa>" + this.HackAndVersionName + "</color>");
		string text = "";
		int num = 0;
		string text2 = "";
		foreach (KeyValuePair<string, bool> keyValuePair in this.categories)
		{
			if ((this.influenceCurrent || this.exitCategory) && this.currentHacksIndex == num)
			{
				text2 = keyValuePair.Key;
				this.exitCategory = false;
			}
			string text3 = "";
			if (num == this.currentHacksIndex)
			{
				text3 = "  <<";
			}
			if (keyValuePair.Value)
			{
				text += this.displayString(string.Concat(new object[]
				{
					keyValuePair.Key,
					" [",
					this.hacks[keyValuePair.Key].Count,
					"] ↓",
					text3,
					"\n"
				}), num);
				num++;
				using (List<BeethovenOption>.Enumerator enumerator2 = this.hacks[keyValuePair.Key].GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						BeethovenOption beethovenOption = enumerator2.Current;
						if (this.influenceCurrent && this.currentHacksIndex == num)
						{
							if (beethovenOption.optionType == BeethovenOptionType.METHOD)
							{
								base.Invoke(beethovenOption.variable, 0f);
							}
							else if (beethovenOption.optionType == BeethovenOptionType.TOGGLE)
							{
								this.setVariableByName(beethovenOption.variable, !(bool)this.getVariableByName(beethovenOption.variable));
							}
							else
							{
								this.setVariableByName(beethovenOption.variable, beethovenOption.getNextCycle());
							}
						}
						text = text + this.displayString("    " + beethovenOption.optionName + beethovenOption.getCurrent(), num) + "\n";
						num++;
					}
					goto IL_257;
				}
				goto IL_1F4;
			}
			goto IL_1F4;
			IL_257:
			if (this.exitCategory && this.currentHacksIndex < num)
			{
				this.categories[keyValuePair.Key] = !keyValuePair.Value;
				this.exitCategory = false;
				continue;
			}
			continue;
			IL_1F4:
			text += this.displayString(string.Concat(new object[]
			{
				keyValuePair.Key,
				" [",
				this.hacks[keyValuePair.Key].Count,
				"]",
				text3,
				"\n"
			}), num);
			num++;
			goto IL_257;
		}
		this.influenceCurrent = false;
		this.currentMaxLines = num;
		if (this.currentHacksIndex >= this.currentMaxLines)
		{
			this.currentHacksIndex = this.currentMaxLines - 1;
		}
		GUI.Label(new Rect(10f, 35f, 1500f, 1500f), text);
		if (text2 != "")
		{
			this.categories[text2] = !this.categories[text2];
		}
	}

	// Token: 0x06001A4C RID: 6732 RVA: 0x0001A4D0 File Offset: 0x000186D0
	private string displayString(string theString, int theLine)
	{
		if (theLine == this.currentHacksIndex)
		{
			return "<color=#FFFFAAFF>" + theString + "</color>";
		}
		return theString;
	}

	// Token: 0x06001A4D RID: 6733 RVA: 0x0001A4ED File Offset: 0x000186ED
	public static void initializeHacks()
	{
		if (GameObject.Find("Beethovens Tools") == null)
		{
			GameObject gameObject = new GameObject("Beethovens Tools");
			gameObject.AddComponent<BeethovenMain>();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}
	}

	// Token: 0x06001A4E RID: 6734 RVA: 0x0009F110 File Offset: 0x0009D310
	public void initializeCategories()
	{
		this.categories = new Dictionary<string, bool>();
		this.hacks = new Dictionary<string, List<BeethovenOption>>();
		foreach (BeethovenOption beethovenOption in this.tools)
		{
			if (!this.categories.ContainsKey(beethovenOption.category))
			{
				this.categories.Add(beethovenOption.category, false);
			}
		}
		foreach (KeyValuePair<string, bool> keyValuePair in this.categories)
		{
			List<BeethovenOption> list = new List<BeethovenOption>();
			foreach (BeethovenOption beethovenOption2 in this.tools)
			{
				if (beethovenOption2.category == keyValuePair.Key)
				{
					list.Add(beethovenOption2);
				}
			}
			this.hacks.Add(keyValuePair.Key, list);
		}
	}

	// Token: 0x06001A4F RID: 6735 RVA: 0x0001A517 File Offset: 0x00018717
	public object getVariableByName(string variableName)
	{
		return base.GetType().GetField(variableName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(this);
	}

	// Token: 0x06001A50 RID: 6736 RVA: 0x0001A52D File Offset: 0x0001872D
	public void setVariableByName(string variableName, object newObject)
	{
		base.GetType().GetField(variableName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(this, newObject);
	}

	// Token: 0x04001C0E RID: 7182
	public string HackAndVersionName = "Beethoven v3.2";

	// Token: 0x04001C0F RID: 7183
	public static BeethovenMain Instance;

	// Token: 0x04001C10 RID: 7184
	public List<BeethovenOption> tools;

	// Token: 0x04001C11 RID: 7185
	public Dictionary<string, bool> categories;

	// Token: 0x04001C12 RID: 7186
	public Dictionary<string, List<BeethovenOption>> hacks;

	// Token: 0x04001C13 RID: 7187
	public int currentHacksIndex;

	// Token: 0x04001C14 RID: 7188
	public int currentMaxLines;

	// Token: 0x04001C15 RID: 7189
	public bool influenceCurrent;

	// Token: 0x04001C16 RID: 7190
	public bool exitCategory;

	// Token: 0x04001C17 RID: 7191
	private Vector3 realPosition;

	// Token: 0x04001C18 RID: 7192
	private List<Door> disabledDoors;

	// Token: 0x04001C19 RID: 7193
	private float lastTeleport;

	// Token: 0x04001C1A RID: 7194
	private ServerRoles mySR;

	// Token: 0x04001C1B RID: 7195
	private PlyMovementSync playerMovement;

	// Token: 0x04001C1C RID: 7196
	private PlayerInteract pi;

	// Token: 0x04001C1D RID: 7197
	private PlayerStats ps;

	// Token: 0x04001C1E RID: 7198
	private CharacterController cc;

	// Token: 0x04001C1F RID: 7199
	private CharacterClassManager myCCM;

	// Token: 0x04001C20 RID: 7200
	private FirstPersonController myFPC;

	// Token: 0x04001C21 RID: 7201
	private WeaponManager myWeapons;

	// Token: 0x04001C22 RID: 7202
	private Searching mySearching;

	// Token: 0x04001C23 RID: 7203
	private MicroHID_GFX myHID;

	// Token: 0x04001C24 RID: 7204
	private Inventory myInventory;

	// Token: 0x04001C25 RID: 7205
	private int someIndex;

	// Token: 0x04001C26 RID: 7206
	private RecoilProperties recoilProperties;

	// Token: 0x04001C27 RID: 7207
	private Vector3 myLastPosition;

	// Token: 0x04001C28 RID: 7208
	private Lift[] lifts;

	// Token: 0x04001C29 RID: 7209
	private PocketDimensionTeleport[] pdts;

	// Token: 0x04001C2A RID: 7210
	private Transform player173;

	// Token: 0x04001C2B RID: 7211
	private Transform cam;

	// Token: 0x04001C2C RID: 7212
	private GameObject target;

	// Token: 0x04001C2D RID: 7213
	private List<Vector3> targetPositions;

	// Token: 0x04001C2E RID: 7214
	public bool flight;

	// Token: 0x04001C2F RID: 7215
	public bool trueFlight;

	// Token: 0x04001C30 RID: 7216
	public bool teleportMode;

	// Token: 0x04001C31 RID: 7217
	public bool maximumSpeed;

	// Token: 0x04001C32 RID: 7218
	public bool playerWallhack;

	// Token: 0x04001C33 RID: 7219
	public bool itemWallhack;

	// Token: 0x04001C34 RID: 7220
	public bool locationsWallhack;

	// Token: 0x04001C35 RID: 7221
	public bool quickItemPickup;

	// Token: 0x04001C36 RID: 7222
	public bool hidSpam;

	// Token: 0x04001C37 RID: 7223
	public bool scpMode;

	// Token: 0x04001C38 RID: 7224
	public bool killTarget;

	// Token: 0x04001C39 RID: 7225
	public bool killEveryone;

	// Token: 0x04001C3A RID: 7226
	public bool keepHandcuffing;

	// Token: 0x04001C3B RID: 7227
	public bool autoPickup;

	// Token: 0x04001C3C RID: 7228
	public bool noRecoil;

	// Token: 0x04001C3D RID: 7229
	public bool doorMaster;

	// Token: 0x04001C3E RID: 7230
	public bool stalk173;

	// Token: 0x04001C3F RID: 7231
	public bool followTarget;

	// Token: 0x04001C40 RID: 7232
	public bool antiTesla;
}
