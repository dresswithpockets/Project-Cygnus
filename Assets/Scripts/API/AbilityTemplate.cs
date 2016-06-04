using UnityEngine;
using System.Collections;
using System;

public struct Ability_Type {
	public Type type;
	public AbilityTier tier;
	public int min_player_level;

	public Ability_Type(Type type, AbilityTier tier, int level) {

		this.type = type;
		this.tier = tier;
		min_player_level = level;
	}
}

public abstract class Ability_Template {
	// Displayed name of the ability
	public abstract string ability_name { get; }

	// Displayed RichText description of the ability
	public abstract string ability_description { get; }

	// Whether or not AI can learn and use this ability
	public abstract bool used_by_AI { get; }

	// The ID of the image that will be used as an avatar for the ability
	public abstract string avatar_ID { get; }

	public abstract int ability_level { get; }

	public virtual void start(PlayerController player) { }

	public virtual void start(NPCController npc) { }

	public virtual void passive_update(PlayerController player) { }

	public virtual void passive_update(NPCController npc) { }

	public virtual void ability_update(PlayerController player) { }

	public virtual void ability_update(NPCController npc) { }

	public virtual void end(PlayerController player) { }

	public virtual void end(NPCController npc) { }

	public virtual bool is_ready() { return true; }

	public virtual bool is_finished() { return true; }

	public virtual void level_up() { }

	public virtual void interrupt() { }

	// Determines whether or not an AI can use this ability on a target
	public virtual bool AI_can_use_on_target(NPCController npc, GameObject target) {

		return false;
	}
}