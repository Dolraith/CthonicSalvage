using Godot;
using System;
using System.Diagnostics;

public partial class Player : Area2D
{
	[Export]
	public int tile_size {get; set;} = 64;
	private RayCast2D ray;
	private bool moving = false;
	private int animation_speed = 3;
	private AnimatedSprite2D sprite;
	private Direction[] directions = new Direction[8];
	Tween moveTween = null;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ray = GetNode<RayCast2D>("RayCast2D");
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		directions[0] = new Direction("move_north", new Vector2(0,-1), "Up");
		directions[1] = new Direction("move_ne", new Vector2(1,-1), "Up");
		directions[2] = new Direction("move_east", new Vector2(1,0), "Right");
		directions[3] = new Direction("move_se", new Vector2(1,1), "Down");
		directions[4] = new Direction("move_south", new Vector2(0,1), "Down");
		directions[5] = new Direction("move_sw", new Vector2(-1,1), "Down");
		directions[6] = new Direction("move_west", new Vector2(-1,0), "Left");
		directions[7] = new Direction("move_nw", new Vector2(-1,-1), "Up");
	}

	public override void _Process(double delta)
	{
		move();

	}
	private void move(){

		if(moving){
			sprite.Play();
			return;
		}
		foreach(Direction dir in directions){
			if(Input.IsActionPressed(dir.input)){
				ray.TargetPosition = dir.vector * tile_size;
				ray.ForceRaycastUpdate();
				if(!ray.IsColliding()){
					moveTween = CreateTween();
					moveTween.TweenProperty(this, "position",	Position + dir.vector * tile_size, 1.0/animation_speed).SetTrans(Tween.TransitionType.Linear);
					moving = true;
					sprite.Animation = dir.animation;
					sprite.Play();
					moveTween.Finished += ()=>{
						moving = false;
						sprite.Stop();
					};
					return;
				}
			}
		}
	}

	internal class Direction {
		public string input;
		public Vector2 vector;
		public string animation;
		public Direction(string input, Vector2 vector, string animation){
			this.input = input;
			this.vector = vector;
			this.animation = animation;
		}
	}
}

